using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemPickup : MonoBehaviour
{
    bool inReach = false;
    public GameObject inventoryItem;
    public GameObject instruction;
    // Start is called before the first frame update
    void Start()
    {
        inventoryItem.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inReach)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventoryItem.SetActive(true);
                instruction.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inReach = true;
            instruction.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            inReach = false;
            instruction.SetActive(false);
        }
    }
}
