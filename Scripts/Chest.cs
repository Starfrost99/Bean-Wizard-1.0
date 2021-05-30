using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public GameObject keys;
    public bool isOpen;
    public GameObject tome;
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!keys.activeSelf && !isOpen) 
            {
                isOpen = true;
                GetComponent<Animator>().SetTrigger("Open");
                GetComponentInChildren<AudioSource>().Play();
                tome.SetActive(true);
                //spawn tome
            }
        }
    }
}
