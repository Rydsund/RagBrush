﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public InventoryObject inventory;
    // Start is called before the first frame update

    public void OnTriggerEnter(Collider other)
    {
        Debug.Log("Found collider");
        var item = other.GetComponent<Item>();
        if (item)
        {
            inventory.AddItem(item.item, 1);
            Destroy(other.gameObject);
        }
    }

    private void OnApplicationQuit()
    {
       // inventory.Container.Clear();
    }
}
