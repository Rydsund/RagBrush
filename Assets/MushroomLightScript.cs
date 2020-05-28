using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomLightScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    bool isLightOn;

    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLightOn)
        {
            GetComponent<Light>().enabled = true;
        }
        else
        {
            GetComponent<Light>().enabled = false;
        }
    }
}
