using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public GameManager GM;
    public GameObject instructions;
    public int healthAmt;
    bool inReach;
    // Start is called before the first frame update
    void Start()
    {
        instructions.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inReach)
        {
            instructions.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                GM.HealPlayer(healthAmt);
                instructions.SetActive(false);
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            inReach = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            inReach = false;
            instructions.SetActive(false);
        }
    }
}
