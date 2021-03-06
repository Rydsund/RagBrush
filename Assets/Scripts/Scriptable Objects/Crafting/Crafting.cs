﻿using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using UnityEngine;

public class Crafting : MonoBehaviour
{
    public Recipe[] craftingRecipies;

    void Start()
    {
        LoadRecipies();
    }

    /// <summary>
    /// Metoden sköter den huvudsakliga crafting-logiken.
    /// Tar emot spelarens två inventory slots och kollar
    /// om detta korresponderar med ett recept.
    /// -- Viktor
    /// </summary>
    public bool Craft(InventorySlot slot1, InventorySlot slot2) 
    {
        List<InventorySlot> craftingSlots = new List<InventorySlot>();
        craftingSlots.Add(slot1);
        craftingSlots.Add(slot2);

        for (int i = 0; i < craftingRecipies.Length; i++)
        {
            if (craftingRecipies[i].inputObj1 == craftingSlots[0].item.objectPrefab
                && craftingRecipies[i].inputObj2 == craftingSlots[1].item.objectPrefab
                || craftingRecipies[i].inputObj1 == craftingSlots[1].item.objectPrefab
                && craftingRecipies[i].inputObj2 == craftingSlots[0].item.objectPrefab)
            {
                var craftedObject = craftingRecipies[i].outputObj;
                craftedObject.transform.position = this.transform.position;
                Instantiate(craftedObject);
                return true;
            }
        }
        return false;
    }


    public bool Craft(InventorySlot slot1 /*InventorySlot slot2*/)
    {
        List<InventorySlot> craftingSlots = new List<InventorySlot>();
        craftingSlots.Add(slot1);
        //craftingSlots.Add(slot2);

        for (int i = 0; i < craftingRecipies.Length; i++)
        {
            if (craftingRecipies[i].inputObj1 == craftingSlots[0].item.objectPrefab
                && craftingRecipies[i].inputObj2 == null)
            {
                var craftedObject = craftingRecipies[i].outputObj;
                craftedObject.transform.position = this.transform.position;
                Instantiate(craftedObject);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Metoden använde Resources.LoadAll för att hämta alla crafting recept
    /// från Resources mappen som finns i Assets.
    /// -- Viktor/Mattias.
    /// </summary>
    private void LoadRecipies()
    {
        craftingRecipies = Resources.LoadAll<Recipe>("");
    }
}
