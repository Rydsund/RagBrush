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


    public bool Craft(InventorySlot slot1, InventorySlot slot2) // För crafting.. duh.
    {
        List<InventorySlot> craftingSlots = new List<InventorySlot>();
        craftingSlots.Add(slot1);
        craftingSlots.Add(slot2);

        for (int i = 0; i < craftingRecipies.Length; i++)
        {
            // Ett första utkast av craftinglogiken. Kollar om a + b = a + b, eller b + a = b + a. Vilket då ger a+b=c, samt b+a=c.
            if (craftingRecipies[i].inputObj1 == craftingSlots[0].item.objectPrefab
                && craftingRecipies[i].inputObj2 == craftingSlots[1].item.objectPrefab
                || craftingRecipies[i].inputObj1 == craftingSlots[1].item.objectPrefab
                && craftingRecipies[i].inputObj2 == craftingSlots[0].item.objectPrefab)
            {
                Instantiate(craftingRecipies[i].outputObj, this.transform);
                // Remove from inventory.
                //Destroy(slot1.item.imagePrefab);
                //Destroy(slot2.item.g);
                return true;
            }
        }
        return false;
                    
    }

    private void LoadRecipies()
    {
        craftingRecipies = Resources.LoadAll<Recipe>("");
        Debug.Log(craftingRecipies.Length);
        Debug.Log(craftingRecipies[0].name);
    }
}
