using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCrosshair : MonoBehaviour
{
    [SerializeField]
    private Transform ragdollHand;

    [SerializeField]
    private Transform ragdoll;

    void FixedUpdate()
    {
        Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        if(Physics.Raycast(rayOrigin, out hitInfo))
        {
            if(hitInfo.collider != null && hitInfo.collider != ragdoll.transform)
            {
                Vector3 direction = hitInfo.point - ragdollHand.position;
                ragdollHand.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
}
