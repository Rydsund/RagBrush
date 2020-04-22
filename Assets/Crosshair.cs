using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour  //Jonathan
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
        //Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hitInfo;


        //if (Physics.Raycast(rayOrigin, out hitInfo))
        //{
        //    if (hitInfo.collider != null)
        //    {

        //        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(hitInfo.point.x, hitInfo.point.y, 100));
        //    }
        //}
    }
    //public void GetCrosshairPoint(Point)
}
