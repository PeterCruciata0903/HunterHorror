using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    // We can change this remotely: Singleton.Instance.PlayerCam.mouseSensitivity;
    public float mouseSensitivity = 400f;
    //We want to change the horizontal transform of the playerBody to match that of the camera
    public Transform playerBody;

    float xRotation = 0f;
    float yRotation = 0f;
    void Start()
    {
        //Locks the cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //Initial camera angle
        //transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //We want to clamp the camera rotation so we can't look behind us.
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //rotate camera and orientation
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        playerBody.rotation = Quaternion.Euler(0, yRotation, 0);


    }
}
