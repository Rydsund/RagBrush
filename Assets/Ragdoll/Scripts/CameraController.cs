using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour //Johan
{
    [SerializeField]
    private float lookSensitivity = 1;
    float mouseX, mouseY;

    Vector3 targetRotation;

    [SerializeField]
    private Transform cameraController, aimController, chest, chestTarget;

    private void Start()
    { 
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void LateUpdate()
    {
        if (PausMenu.gamePaused)
            return;
        CamControl();
    }

    void CamControl()
    {
        mouseY += Input.GetAxisRaw("Mouse Y") * lookSensitivity;
        mouseX -= Input.GetAxisRaw("Mouse X") * lookSensitivity;
        mouseY = Mathf.Clamp(mouseY, -35, 18);
       
        //bendPart.rotation = Quaternion.Euler(0, 0, mouseY);

        cameraController.rotation = Quaternion.Euler(-mouseY, -mouseX, 0);

        //aimController.rotation = Quaternion.Euler(-mouseY, , 0);
        //bendController.rotation = Quaternion.Euler(-mouseY, 0, 0);
        transform.LookAt(cameraController);


        Aim();

    }

    void Aim()
    {
        //mouseX = Mathf.Clamp(mouseX, 0, 0);
        //aimController.rotation.y = cameraController.rotation.y;

        //targetRotation = Quaternion.LookRotation(chest.position).eulerAngles;

        //Quaternion newRotation;

        //newRotation/*aimController.rotation*/ = Quaternion.Euler(-mouseY, chest.rotation.x, chest.rotation.z);

        aimController.Rotate(-mouseY/*newRotation.x*//*aimController.rotation.x*/, aimController.rotation.y/*newRotation.x*/, aimController.rotation.z /*newRotation.z*/, Space.Self);

        //aimController.rotation = Quaternion.Euler(-mouseY, aimController.rotation.y, aimController.rotation.z);



    }
}
