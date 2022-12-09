using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_controller : MonoBehaviour
{
    public Transform player;
    private float mouseX, mouseY; //get mouse moving value
    public float mounseSensitivity; // mounse Sensitivity
    public float xRotation;

    private void Start()
    {   // lock the mounse so it would not go out of the game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void Update()
    {
        //get the mouse position change in X and Y
        mouseX = Input.GetAxis("Mouse X") * mounseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mounseSensitivity * Time.deltaTime;

        //perspective moves up or down is decide by mouseY , rotate around X axis
        xRotation -= mouseY; // +,- can decide the direction about up and down

        //set an scale on move up and down
        xRotation = Mathf.Clamp(xRotation, -100f, 100f); //-70f min , 70 max
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //since camera is set as chile of the player
        //so for horizontal rotation, we rotate player (the camera would also rotate)
        player.Rotate(Vector3.up * mouseX);
    }
}
