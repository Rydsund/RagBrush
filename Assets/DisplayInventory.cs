using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using Vector3 = UnityEngine.Vector3;
using Quaternion = UnityEngine.Quaternion;

public class DisplayInventory : MonoBehaviour
{

    public InventoryObject inventory;
    public int x_SpaceBetweenItems;
    public int number_Of_Columns;
    public int y_SpaceBetweenItems;
    Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    // Start is called before the first frame update
    void Start()
    {

        CreateDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.prefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
        }
    }

    public Vector3 GetPosition(int i)
    {
        return new Vector3(x_SpaceBetweenItems * (i % number_Of_Columns), (-y_SpaceBetweenItems * (i/number_Of_Columns)), 0f);
    }
}
