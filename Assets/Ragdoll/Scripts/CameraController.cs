﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lookSensitivity = 1;
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
        mouseY += Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        mouseX -= Input.GetAxisRaw("Mouse X") * lookSensitivity;
        mouseY = Mathf.Clamp(mouseY, -35, 20);

        transform.LookAt(target);

        target.rotation = Quaternion.Euler(-mouseY, -mouseX, 0);
        target.position = player.position;
    }
}
