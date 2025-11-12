using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour {
    //public Transform player;
    //public float mouseSensitivity = 2f;
    //float cameraVerticalRotation = 0f;

    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    void Start() {
        // Lock and Hide the Cursor
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // Collect Mouse Input
        float inputX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
        float inputY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

        yRotation += inputX;
        xRotation -= inputY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        // Rotate the Camera around its local X axis
        //cameraVerticalRotation -= inputY;
        //cameraVerticalRotation = Mathf.Clamp(cameraVerticalRotation, -90f, 90f);
        //transform.localEulerAngles = Vector3.right * cameraVerticalRotation;

        // Rotate the Player Object and the Camera around its Y axis
        //player.Rotate(Vector3.up * inputX);
    }
}
