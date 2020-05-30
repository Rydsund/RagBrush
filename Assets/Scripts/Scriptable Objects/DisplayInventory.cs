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
    public Dictionary<InventorySlot, GameObject> itemsDisplayed = new Dictionary<InventorySlot, GameObject>();

    [SerializeField]
    private int x_Start;
    [SerializeField]
    private int y_Start;
    [SerializeField]
    private int x_SpaceBetweenItems = 250;
    [SerializeField]
    private int number_Of_Columns = 4;
    [SerializeField]
    private int y_SpaceBetweenItems = 150;

    void Start()
    {
        CreateDisplay(); // Initierar display.
    }

    void Update()
    {
        UpdateDisplay();
    }

    /// <summary>
    /// Update display har till uppgift att hela tiden visa en korrekt grafisk representation
    /// av items som existerar i ett särskilt inventory. Kan även användas för andra inventorys förutom spelarens.
    /// T.e.x. ett Tutorial-inventory.
    /// -- Viktor
    /// </summary>
    public void UpdateDisplay()
    {
        for (int i = 0; i < inventory.Container.Count; i++)
        {
            if (itemsDisplayed.ContainsKey(inventory.Container[i]))
            {   
            }
            else
            {   // Annars skapas en grafisk representation av inventory.
                var obj = Instantiate(inventory.Container[i].item.imagePrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);            
                //obj.GetComponentInChildren<TextMeshProUGUI>().text = inventory.Container[i].amount.ToString("n0");                           
                itemsDisplayed.Add(inventory.Container[i], obj);
            }
        }

    }
    
    /// <summary>
    /// Har till uppgift att vid start visa dem items som redan existerar i ett särskilt inventory. 
    /// Just nu används inte detta. Kan dock användas till Tutorial.
    /// -- Viktor
    /// </summary>
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

    /// <summary>
    /// Metoden har till uppgift att skapa en grid-layout utav item.imagePrefab för displayInventory.
    /// Detta används istället för en GridLayout-Group i Canvas.
    /// -- Viktor
    /// </summary>
    public Vector3 GetPosition(int i)
    {
        return new Vector3(x_Start + (x_SpaceBetweenItems * (i % number_Of_Columns)), y_Start + ((-y_SpaceBetweenItems * (i/number_Of_Columns))), 0f);
    }
}
