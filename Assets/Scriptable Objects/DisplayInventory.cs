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

    public int x_Start;
    public int y_Start;

    public int x_SpaceBetweenItems;
    public int number_Of_Columns;
    public int y_SpaceBetweenItems;
   public Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();
    

    void Start()
    {
        CreateDisplay(); // Initierar display.
    }

    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
    }
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {
                itemsDisplayed[inventory.Container[i]].GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            }
            else
            {
                var obj = Instantiate(inventory.Container[i].item.imagePrefab, Vector3.zero, Quaternion.identity, transform);
               
                // Sätter pos
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
               
                obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
               
                
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }

        // For dropping items. Consider moving to player.
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    //inventory.Container.Clear();
        //    //itemsDisplayed.Clear();       
        //    for (int i = 0; i < itemsDisplayed.Count; i++)
        //    {
        //        Destroy(itemsDisplayed[inventory.Container[i]].gameObject);
        //        inventory.Container.RemoveAt(i);
        //    }
        //}

    }

    public void CreateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            var obj = Instantiate(inventory.Container[i].item.imagePrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");
            itemsDisplayed.Add(inventory.Container[i], obj);
        }
    }

    public Vector3 GetPosition(int i)
    { // grid layout
        return new Vector3(x_Start + (x_SpaceBetweenItems * (i % number_Of_Columns)), y_Start + ((-y_SpaceBetweenItems * (i/number_Of_Columns))), 0f);
    }
}
