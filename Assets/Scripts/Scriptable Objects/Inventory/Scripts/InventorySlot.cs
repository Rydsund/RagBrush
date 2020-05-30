using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// InventorySlot är en klass/objekt som endast håller ett item och en mängd.
/// Är byggstenar för spelarens Inventory.
/// -- Viktor
/// </summary>
[System.Serializable]
public class InventorySlot
{
    public ItemObject item;
    public int amount;

    public InventorySlot(ItemObject _item, int _amount)
    {
        item = _item;
        amount = _amount;
    }

    public void AddAmount(int value)
    {
        amount += value;
    }
}
