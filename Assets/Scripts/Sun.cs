using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 
/// /Johan
/// </summary>
public class Sun : MonoBehaviour
{
    Light sunShine;

    [SerializeField]
    private float dayTime = 1;

    private void Start()
    {
        sunShine = GetComponent<Light>();
    }


    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right, dayTime * Time.deltaTime);
        transform.LookAt(Vector3.zero);

        if (transform.position.y > -150 && sunShine.intensity < 1f)
        {
            sunShine.intensity += 0.1f;
        }
        else if (transform.position.y < -150 && sunShine.intensity > 0.1f)
        {
            sunShine.intensity -= 0.1f;
        }
    }
    
    
}
