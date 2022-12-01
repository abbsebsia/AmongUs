using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    // Start is called before the first frame update
    private Vector3 velocity;
    [SerializeField] Camera camera;
    [SerializeField] private float speed = 3;
    [SerializeField] private Rigidbody player;

    public float sensitivityX = 15F;
    public float sensitivityY = 15F;
    public float minimumX = -360F;
    public float maximumX = 360F;
    public float minimumY = -60F;
    public float maximumY = 60F;
    float rotationX = 0F;
    float rotationY = 0F;
    Quaternion originalRotation;

    void Start() {
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame

    void Update() {

        //Get the Screen positions of the object

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);   
        camera.transform.localRotation = originalRotation * xQuaternion * yQuaternion;
    }

    private void FixedUpdate() {
        // Move left right with A and D, forward back with W and S
        // rotate according to camera local eulerangles?
        float xVel = Input.GetAxisRaw("Horizontal");
        float zVel = Input.GetAxisRaw("Vertical");
        float angle = -camera.transform.localEulerAngles.y * Mathf.Deg2Rad;

        Vector3 velocity = new Vector3(xVel * Mathf.Cos(angle) - zVel * Mathf.Sin(angle), 0, xVel * Mathf.Sin(angle) + zVel * Mathf.Cos(angle)) * speed;
        player.velocity = velocity;
        //Debug.Log(xVel);
        //Debug.Log(zVel);
        Debug.Log(player.velocity.x);
        Debug.Log(player.velocity.z);
        camera.transform.position = transform.position;
    }

    private float ClampAngle(float angle, float min, float max) {
        angle %= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
