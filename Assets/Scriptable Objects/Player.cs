﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public InventoryObject inventory;
    [SerializeField] DisplayInventory displayInventory;
    [SerializeField] Crafting crafting;

    /// <summary>
    /// En OnTriggerEnter som kollar om spelaren kolliderat med ett item.
    /// Om så är fallet så läggs detta item i inventory och spelobjektet förstörs.
    /// Viktor
    /// </summary>
    //public void OnTriggerEnter(Collider other)
    //{
    //    AddItemToInventory(other);
    //}

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) // Crafting
        {
            CraftItem();
        }

        if (Input.GetKeyDown(KeyCode.N)) // Dropping items
        {
            DropItem();
        }
    }

    /// <summary>
    /// Metoden anropar Craft(), och skickar med spelarens två inventory slots som inparameter.
    /// Craft() har till uppgift att avgöra om dessa två items överrenstämmer med ett recept.
    /// Om så är fallet skapas det ett spelobjekt och spelarens inventory töms.
    /// Viktor
    /// </summary>
    private void CraftItem()
    {
        if (inventory.Container.Count > 1 && inventory.Container.Count < 3) // Endast crafting om två items i inventory.
        {
            if (crafting.Craft(inventory.Container[0], inventory.Container[1]) == true) // Craft är en metod som returnerar en bool.
            {
                Destroy(displayInventory.itemsDisplayed[inventory.Container[0]].gameObject);
                Destroy(displayInventory.itemsDisplayed[inventory.Container[1]].gameObject);
                inventory.Container.Clear();
            }
        }
    }


    /// <summary>
    /// Metoden släpper ett item i taget från spelarens inventory, från vänster till höger.
    /// Itemet instansieras sedan vid spelarens position
    /// -- Viktor / Matthias.
    /// </summary>
    private void DropItem()
    {
        for (int i = 0; i < displayInventory.itemsDisplayed.Count; i++)
        {
            var tempObj = inventory.Container[i].item.objectPrefab;
            tempObj.transform.position = this.transform.position;
            Instantiate(tempObj);

            Destroy(displayInventory.itemsDisplayed[inventory.Container[i]].gameObject);
            inventory.Container.RemoveAt(i);
        }
    }

    public void AddItemToInventory(Collider collider)
    {
        //collider.isTrigger = true;
        Debug.Log("Found collider");
        var item = collider.GetComponent<Item>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(collider.gameObject);
        }
    }

    // För att tömma inventory vid quit.
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
