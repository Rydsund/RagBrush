using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public Recipe[] craftingRecipies;

    void Start() // För att läsa in recept.
    {
        LoadRecipies();
    }


    public void Craft(InventorySlot slot1, InventorySlot slot2) // För crafting.. duh.
    {
        List<InventorySlot> craftingSlots = new List<InventorySlot>();
        craftingSlots.Add(slot1);
        craftingSlots.Add(slot2);

        //for (int i = 0; i < 1; i++)
        //{
        //    if (craftingSlots[i] == )
        //    {

        //    }
        //}

        // Ett första utkast av craftinglogiken. Kollar om a + b = a + b, eller b + a = b + a.
        if (craftingRecipies[0].inputObj1 == craftingSlots[0].item.objectPrefab
            && craftingRecipies[0].inputObj2 == craftingSlots[1].item.objectPrefab
            || craftingRecipies[0].inputObj1 == craftingSlots[1].item.objectPrefab
            && craftingRecipies[0].inputObj2 == craftingSlots[0].item.objectPrefab)
        {
            Instantiate(craftingRecipies[0].outputObj, this.transform);
            // Remove from inventory.
        }                        
    }

    private void LoadRecipies()
    {
        craftingRecipies = Resources.LoadAll<Recipe>("");
        Debug.Log(craftingRecipies.Length);
        Debug.Log(craftingRecipies[0].name);
    }
}
