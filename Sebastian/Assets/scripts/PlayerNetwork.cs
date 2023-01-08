using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;
using Unity.Netcode;
using Unity.Collections;

public class PlayerNetwork : NetworkBehaviour
{
    // Start is called before the first frame update
    private Vector3 velocity;
    [SerializeField] private Camera camera;
    [SerializeField] Animator animator;
    [SerializeField] private float speed = 3;
    [SerializeField] private Rigidbody player;

    [SerializeField] private Transform spawnedObjectPrefab;

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -40F;
    public float maximumY = 40F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;

    private float xVel;
    private float zVel;

    bool isDead = false;

    private NetworkVariable<MyCustomData> randomNumber = new NetworkVariable<MyCustomData>(new MyCustomData { _int = 32, _bool = true }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public struct MyCustomData : INetworkSerializable
    {
        public int _int;
        public bool _bool;
        public FixedString128Bytes message; 

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T: IReaderWriter
        {
            serializer.SerializeValue(ref _int);
            serializer.SerializeValue(ref _bool);
            serializer.SerializeValue(ref message);
        }

    }
    void Start() {
        
        if (!IsOwner)
        {
            return;
        }
        Application.targetFrameRate = 240;
        originalRotation = transform.localRotation;
    }

    public override void OnNetworkSpawn()
    {
        randomNumber.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + "; " + newValue._int + "; " + newValue._bool + " ; " + newValue.message);
        };
    }

    // Update is called once per frame

    private void Update() {
        
       
        //Get the Screen positions of the object
        if (!IsOwner) {
            Camera cam = GetComponentInChildren<Camera>();
            cam.enabled = false;
            return;
        }
       
        xVel = Input.GetAxisRaw("Horizontal");
        zVel = Input.GetAxisRaw("Vertical");
        //if (Input.GetMouseButtonDown(0)) Die();
        if (Input.GetKeyDown(KeyCode.T))
        {
            Transform spawnedObjectTransform = Instantiate(spawnedObjectPrefab);
            spawnedObjectTransform.GetComponent<NetworkObject>().Spawn(true);

            //TestClientRpc(new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new List<ulong> {1} }  });
            // TestServerRpc(new ServerRpcParams());
            /*
            randomNumber.Value = new MyCustomData
            {
                _int = 10,
                _bool = !randomNumber.Value._bool,
                message = "hej ehj",
            };
            */
        }
        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);

        Transform cameraTransform = transform.Find("Camera");
        cameraTransform.localRotation = originalRotation * xQuaternion * yQuaternion;

        
        player.transform.localEulerAngles = new Vector3(0, cameraTransform.localEulerAngles.y, 0);
    }

    private void FixedUpdate() {

        if (!IsOwner)
        {
            return;
        }
        Transform cameraTransform = transform.Find("Camera");

        float angle = -cameraTransform.localEulerAngles.y * Mathf.Deg2Rad;
        Vector3 velocity = new Vector3(xVel * Mathf.Cos(angle) - zVel * Mathf.Sin(angle), 0, xVel * Mathf.Sin(angle) + zVel * Mathf.Cos(angle)) * speed;
        if (velocity.magnitude > 1e-4) animator.SetBool("running", true);
        else animator.SetBool("running", false);
        // Move left right with A and D, forward back with W and S
        //transform.eulerAngles.Set(transform.eulerAngles.x, angle * Mathf.Rad2Deg, transform.eulerAngles.z);
        if (player.velocity != velocity) player.velocity = velocity;
        
        //Debug.Log(xVel);
        //Debug.Log(zVel);
        //Debug.Log(player.velocity.x);
        //Debug.Log(player.velocity.z);
    }

    private float ClampAngle(float angle, float min, float max) {
        angle %= 360;
        return Mathf.Clamp(angle, min, max);
    }

    public void Die() {
        isDead = !isDead;
        GetComponent<MeshRenderer>().enabled = !isDead; // Invisible
        GetComponent<BoxCollider>().enabled = !isDead; // No collision
        GetComponent<Rigidbody>().useGravity = !isDead; // No gravity
        Debug.Log(isDead);
    }


    [ServerRpc]
    private void TestServerRpc(ServerRpcParams serverRpcParams)
    {
        Debug.Log("test server rpc " + OwnerClientId + " ; " + serverRpcParams.Receive.SenderClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("test client rpc ");
    }
}
