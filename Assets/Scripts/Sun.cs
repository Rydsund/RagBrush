using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{

    [SerializeField]
    private float daytime = 1;

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, daytime * Time.deltaTime);
        transform.LookAt(Vector3.zero);
    }
}
