using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControll : MonoBehaviour
{
    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float rotX;
    float rotY;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        PlayerInput();

        cam.transform.localRotation = Quaternion.Euler(rotX, rotY, 0);
        orientation.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }

    void PlayerInput()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        rotY += mouseX * sensX * multiplier;
        rotX -= mouseY * sensY * multiplier;

        rotX = Mathf.Clamp(rotX, -90f, 90f);
    }
}
