using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public GameObject inputObj1;
    public GameObject inputObj2;
    public GameObject outputObj;

    [TextArea(15, 20)]
    public string description;
}
