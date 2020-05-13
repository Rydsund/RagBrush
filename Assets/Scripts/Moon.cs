using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// /Johan
/// </summary>
public class Moon : MonoBehaviour
{
    Light moonShine;

    [SerializeField]
    private float nightTime = 1;

    private void Start()
    {
        moonShine = GetComponent<Light>();
    }


    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, nightTime * Time.deltaTime);
        transform.LookAt(Vector3.zero);

        if (transform.position.y > -150 && moonShine.intensity < 0.3f)
        {
            moonShine.intensity += 0.1f;
        }
        else if(transform.position.y < -150 && moonShine.intensity > 0.1f)
        {
            moonShine.intensity -= 0.1f;
        }
    }
}
