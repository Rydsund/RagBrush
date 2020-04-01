using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle how objects are destoryed while the game is running.
/// Created by Mattias Smedman
/// </summary>

public class Destructible : MonoBehaviour
{
    public GameObject replacement;

    [Tooltip("Leave null if object should be breakable by everything")]
    public GameObject onlyBreakableBy;

    [Range(0, 100)]
    public float resistance;

    private void Start()
    {
        replacement.transform.localScale = transform.localScale;
    }

    /// <summary>
    /// Destroys the object when a certain amount of force is applied to it. 
    /// If the object is only breakable by one specific object, it will not break in this way from any other object.
    /// </summary>
    /// <param name="c">Object that current object has collided with</param>
    private void OnCollisionEnter(Collision c)
    {
        if (onlyBreakableBy == null || c.gameObject == onlyBreakableBy)
        {
            if((c.relativeVelocity.x > resistance || c.relativeVelocity.y > resistance || c.relativeVelocity.z > resistance))
            {
                Instantiate(replacement, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
