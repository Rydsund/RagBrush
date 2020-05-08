using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour //Johan
{
    [SerializeField]
    private float lookSensitivity = 1;
    float mouseX, mouseY;

    [SerializeField]
    private Transform target, player;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void LateUpdate()
    {
        CamControl();
    }

    void CamControl()
    {
        mouseY += Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        mouseX -= Input.GetAxisRaw("Mouse X") * lookSensitivity;
        mouseY = Mathf.Clamp(mouseY, -35, 18);

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(-mouseY, -mouseX, 0);
    }
}
