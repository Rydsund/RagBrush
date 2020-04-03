using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float rotationSpeed = 1;
    public Transform target, player;
    float mouseX, mouseY;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        CamControl();
    }

    void CamControl()
    {
        mouseY += Input.GetAxisRaw("Mouse Y") * rotationSpeed;
        mouseX -= Input.GetAxisRaw("Mouse X") * rotationSpeed;
        mouseY = Mathf.Clamp(mouseY, -35, 25);

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(-mouseY, -mouseX, 0);
        target.position = player.position;
        //player.rotation = Quaternion.Euler(0, mouseX, 0);

    }
}
