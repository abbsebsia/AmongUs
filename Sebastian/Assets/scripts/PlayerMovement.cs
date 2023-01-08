using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


public class PlayerMovement : NetworkBehaviour
{

    [SerializeField] float mouseSensitivity = 100f;
    [SerializeField] float speed = 15f;
    [SerializeField] float gravity = -9.81f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDistance = 0.2f;
    [SerializeField] LayerMask groundMask;

    [SerializeField] Animator animator;

    Vector3 velocity;
    bool isGrounded;
    bool hasExited = false;

    [SerializeField] Transform playerBody;

    [SerializeField] Transform cameraTransform;

    [SerializeField] CharacterController controller;
   

    float xRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 160;
        if (!IsOwner) return;
        Cursor.lockState = CursorLockMode.Locked;
        playerBody.position = new Vector3(0f, 0.2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            Camera cam = playerBody.GetComponentInChildren<Camera>();
            cam.enabled = false;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            hasExited = !hasExited;
        }

        GroundCheck();
        Move();

        velocity.y += gravity * Time.deltaTime * Time.deltaTime;
        controller.Move(velocity);

        if (!hasExited)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Look();
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = playerBody.right * x + playerBody.forward * z;

        controller.Move(speed * Time.deltaTime * move);
        if (x != 0 || z != 0) animator.SetBool("running", true);
        else animator.SetBool("running", false);
    }

    private void Look()
    {


        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -50f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        playerBody.Rotate(Vector3.up * mouseX);
    }

    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }
}
