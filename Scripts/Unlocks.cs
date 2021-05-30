/* 
 * author : Alex Holdo
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unlocks : MonoBehaviour
{
    public float health = 3;
    public bool isAlive = true;
    public GameObject door;
    //public ParticleSystem death;
    //public AudioSource sfx;
    public GameObject death;
    public GameObject sfx;
    public GameObject lock1;
    public int runs = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0f)
        {
            isAlive = false;
        }
        if (isAlive == false)
        {
            //visually kill enemy here
            RunOnce();
            //gameObject.SetActive(false);

        }
    }

    private void RunOnce()
    {
        if (runs == 0)
        {
            door.GetComponent<Animator>().SetTrigger("Open");
            death.GetComponent<ParticleSystem>().Play();
            sfx.GetComponent<AudioSource>().Play();
            lock1.SetActive(false);
            runs++;
        }
        
    }
}
