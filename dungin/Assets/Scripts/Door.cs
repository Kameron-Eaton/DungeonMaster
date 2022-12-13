using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameManager GM;
    bool inReach = false;
    public GameObject interact;
    // Start is called before the first frame update
    void Start()
    {
        interact.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inReach && Input.GetKeyDown(KeyCode.E))
        {
            GM.NextLevel();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            interact.SetActive(true);
            inReach = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            interact.SetActive(false);
            inReach = false;
        }
    }
}
