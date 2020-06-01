using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public List<InventorySlot> Container = new List<InventorySlot>();
    //int count = 0;

    /// <summary>
    ///  Metoden har till uppgift att lägga till en inventory Slot i en lista(Container) av Inventory Slots 
    ///  förutsatt att spelaren inte redan har plockat upp itemet. Har spelaren redan ett item av den typen
    ///  så sätts HasItem till true och spelaren plockar inte upp itemet.
    ///  -- Viktor
    /// </summary>
    public void AddItem(ItemObject _item, int _amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item == _item)
            {
                hasItem = true;
                break;
            }
        }

        if (!hasItem && Container.Count <= 1)
        {
            //count++;
            Container.Add(new InventorySlot(_item, _amount));
        }
    }
}

