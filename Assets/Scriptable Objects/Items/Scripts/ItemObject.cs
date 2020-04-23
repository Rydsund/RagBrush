using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ItemObjekt är basklassen för alla Scriptable Objects som är items.
/// -- Viktor
/// </summary>
public enum ItemType
{
    Food,
    Equipment,
    Default
}
public abstract class ItemObject : ScriptableObject // Basklass för Items
{
    public GameObject imagePrefab;
    public GameObject objectPrefab;
    public ItemType type;
    [TextArea(15,20)]
    public string description;

}
