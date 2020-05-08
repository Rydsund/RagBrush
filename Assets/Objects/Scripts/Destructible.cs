using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to handle how objects are destoryed while the game is running.
/// Created by Mattias Smedman
/// </summary>

public class Destructible : MonoBehaviour
{
    [SerializeField]
    public GameObject replacement;

    [SerializeField]
    [Tooltip("Leave empty if object should be breakable by everything")]
    string onlyBreakableBy;

    [SerializeField]
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
    /// <param name="collider">Object that current object has collided with</param>
    private void OnCollisionEnter(Collision collider)
    {
        string colliderTag = collider.gameObject.tag;
        if (onlyBreakableBy == null || colliderTag == onlyBreakableBy)
        {
            if((collider.relativeVelocity.x > resistance || collider.relativeVelocity.y > resistance || collider.relativeVelocity.z > resistance))
            {
                Instantiate(replacement, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
