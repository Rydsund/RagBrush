using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public InventoryObject inventory;
    [SerializeField] Button button;
    [SerializeField] DisplayInventory displayInventory;
    [SerializeField] Crafting crafting;
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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (inventory.Container.Count > 1 && inventory.Container.Count < 3) // Endast crafting om två items i inventory.
            {
                crafting.Craft(inventory.Container[0], inventory.Container[1]);
            }

        }

        //if (Input.GetKeyDown(KeyCode.F1))
        //{
        //    var tempObj = inventory.Container[0].item.objectPrefab;
        //    tempObj.transform.position = this.transform.position;
        //    Instantiate(tempObj);
        //}


        // For dropping items.
        if (Input.GetKeyDown(KeyCode.N))
        {
            //inventory.Container.Clear();
            //itemsDisplayed.Clear();       
            for (int i = 0; i < displayInventory.itemsDisplayed.Count; i++)
            {
                var tempObj = inventory.Container[i].item.objectPrefab;
                tempObj.transform.position = this.transform.position;
                Instantiate(tempObj);

                Destroy(displayInventory.itemsDisplayed[inventory.Container[i]].gameObject);
                inventory.Container.RemoveAt(i);                    
            }
        }

    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
