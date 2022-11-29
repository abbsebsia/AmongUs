using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {
    // Start is called before the first frame update
    private Vector3 velocity;
    [SerializeField] Camera camera;
    [SerializeField] private float speed = 3;
    [SerializeField] private Rigidbody player;
    private float fieldOfView;

    void Start() {
        fieldOfView = camera.fieldOfView;
    }

    // Update is called once per frame
    void Update() {

        //Debug.Log("velocity:");
        //Debug.Log(player.velocity);
        //Debug.Log("axis:");
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        //Debug.Log(Input.GetAxisRaw("Vertical"));

        //Get the Screen positions of the object

        float screenWidth = Screen.width;
        float xDiff = screenWidth / 2.0f - Input.mousePosition.x;



        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(camera.transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Debug.Log(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //Ta Daaa
        //camera.transform.rotation = Quaternion.Euler(new Vector3(0f, angle));
    }

    private void FixedUpdate() {
        // Move left right with A and D, 
        player.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * speed;
        camera.transform.position = transform.position;
    }

    private float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }
}
