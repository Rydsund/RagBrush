using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public InventoryObject inventory;
    [SerializeField] Button button;
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
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventory.Container.Clear();
        }

        if (Input.GetKeyDown(KeyCode.F1))
        {
            var tempObj = inventory.Container[0].item.objectPrefab;
            tempObj.transform.position = this.transform.position;
            Instantiate(tempObj);
        }

        //if ()
        //{

        //}

    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
