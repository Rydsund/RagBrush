using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyedObjectTimer : MonoBehaviour
{
    // Viktor.
    /// <summary>
    /// Class is a simple timer for destructible objects
    /// </summary>

    float objectLifeTime;
    Random random;
    void Start()
    {
        objectLifeTime = Random.Range(15, 30);
    }

    // Update is called once per frame
    void Update()
    {
        objectLifeTime -= Time.deltaTime;
        if (objectLifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
