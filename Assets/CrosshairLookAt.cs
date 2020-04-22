using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairLookAt : MonoBehaviour        //Jonathan
{
    [SerializeField]
    private Transform ragdollPartToManipulate;

    [SerializeField]
    private Transform ragdoll;

    private Camera ragdollCam;

    void Start()
    {
        ragdollCam = GetComponentInParent<Camera>();
    }
 

    void FixedUpdate()
    {
        //Vector3 myScreenPos = Camera.main.WorldToScreenPoint(transform.position);
        //transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, myScreenPos.z));
        Ray rayOrigin = ragdollCam.ScreenPointToRay(Input.mousePosition);
        //Vector3 rayOrigin = ragdollCam.ViewportToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
        RaycastHit hitInfo;
        

        if(Physics.Raycast(rayOrigin, out hitInfo))
        {
            if(hitInfo.collider != null)
            {
                Vector3 direction = hitInfo.point - ragdollPartToManipulate.position;
                
                ragdollPartToManipulate.rotation = Quaternion.LookRotation(direction);
                
            }
        }
    }
}
