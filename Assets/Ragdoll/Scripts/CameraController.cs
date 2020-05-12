using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// /Johan
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float lookSensitivity = 1;
    float mouseX, mouseY, mouseLookX;

    [SerializeField]
    private Transform cameraController, aimController, player;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        if (PausMenu.gamePaused)
            return;
        CamControl();
        AimControll();
    }

    void CamControl()
    {
        mouseY += Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        mouseX -= Input.GetAxisRaw("Mouse X") * lookSensitivity;
        mouseY = Mathf.Clamp(mouseY, -35, 18);

        transform.LookAt(cameraController);

        cameraController.rotation = Quaternion.Euler(-mouseY, -mouseX, 0);
    }
   
    void AimControll()
    {
        aimController.eulerAngles = new Vector3(/*transform.eulerAngles.x*/-mouseY * 3f, player.eulerAngles.y, player.eulerAngles.z);
    }
}
