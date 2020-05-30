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
    GameObject replacement;

    [SerializeField]
    [Tooltip("Leave empty if object should be breakable by everything")]
    string onlyBreakableBy;

    [SerializeField]
    [Range(0, 100)]
    float resistance;

    private void Start()
    {
        replacement.transform.localScale = transform.localScale;
    }

    /// <summary>
    /// Förstör objektet när det kolliderar med ett annat objekt med en viss relativ hastighet.
    /// Om objektet endast kan förstöras av objekt med en specifik tag så händer inget om det kolliderande objektet ej har den tagen.
    /// </summary>
    /// <param name="collider">Object that current object has collided with</param>
    private void OnCollisionEnter(Collision collider)
    {
        string colliderTag = collider.gameObject.tag;
        if (onlyBreakableBy == "" || colliderTag == onlyBreakableBy)
        {
            if((collider.relativeVelocity.x > resistance || collider.relativeVelocity.y > resistance || collider.relativeVelocity.z > resistance))
            {
                Instantiate(replacement, transform.position, transform.rotation);
                Destroy(gameObject);
            }
        }
    }
}
