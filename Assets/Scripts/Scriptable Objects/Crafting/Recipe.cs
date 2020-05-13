using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Recipie är ett Scriptable Object som används för att skapa crafting recipies.
/// Tar två input, och har ett output.
/// -- Viktor.
/// </summary>
[CreateAssetMenu]
public class Recipe : ScriptableObject
{
    public GameObject inputObj1;
    public GameObject inputObj2;
    public GameObject outputObj;

    [TextArea(15, 20)]
    public string description;
}
