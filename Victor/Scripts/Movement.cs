using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour {
    // Start is called before the first frame update
    private Vector3 velocity;
    [SerializeField] private Camera camera;
    [SerializeField] Animator animator;
    [SerializeField] private float speed = 3;
    [SerializeField] private Rigidbody player;

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

    void Start() {
        originalRotation = transform.localRotation;
    }

    // Update is called once per frame

    void Update() {
        //Get the Screen positions of the object

        xVel = Input.GetAxisRaw("Horizontal");
        zVel = Input.GetAxisRaw("Vertical");
        //if (Input.GetMouseButtonDown(0)) Die();

        rotationX += Input.GetAxis("Mouse X") * sensitivityX;
        rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
        rotationX = ClampAngle(rotationX, minimumX, maximumX);
        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);   
        camera.transform.localRotation = originalRotation * xQuaternion * yQuaternion;

        Transform sprite = GameObject.FindGameObjectWithTag("sprite").GetComponent<Transform>();
        float angle = camera.transform.localEulerAngles.y;
        sprite.localEulerAngles = new Vector3(0, angle, 0);
    }

    private void FixedUpdate() {
        // Move left right with A and D, forward back with W and S
        float angle = -camera.transform.localEulerAngles.y * Mathf.Deg2Rad;
        //transform.eulerAngles.Set(transform.eulerAngles.x, angle * Mathf.Rad2Deg, transform.eulerAngles.z);
        Vector3 velocity = new Vector3(xVel * Mathf.Cos(angle) - zVel * Mathf.Sin(angle), 0, xVel * Mathf.Sin(angle) + zVel * Mathf.Cos(angle)) * speed;
        if (player.velocity != velocity) player.velocity = velocity;
        if (velocity.magnitude > 1e-4) animator.SetBool("running", true);
        else animator.SetBool("running", false);
        //Debug.Log(xVel);
        //Debug.Log(zVel);
        //Debug.Log(player.velocity.x);
        //Debug.Log(player.velocity.z);
        camera.transform.position = transform.position;
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
}
