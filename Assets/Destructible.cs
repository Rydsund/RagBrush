using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    public GameObject replacement;
    public float resistance;

    private void Start()
    {
        replacement.transform.localScale = transform.localScale;
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.relativeVelocity.x > resistance || c.relativeVelocity.y > resistance || c.relativeVelocity.z > resistance)
        {
            Instantiate(replacement, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
