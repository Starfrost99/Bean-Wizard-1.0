/* 
 * author : Alex Holdo
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using TMPro

public class Spell : MonoBehaviour
{

    public ParticleSystem ps;
    List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    public bool isCast = false;
    public float cdTime =  999f;
    public float cooldown = 0;
    public AudioSource[] sfx = new AudioSource[10];

    [Range(1, 6)]
    public int spellType;
    public float damage;
    public GameObject[] unlockables;

    private Animator wand;

    public Image icon_button;
    public Image icon_image;

    public int hits = 0;
    public bool reset = false;

    // Start is called before the first frame update
    void Start()
    {
        wand = transform.parent.parent.Find("Wand Pivot").gameObject.GetComponent<Animator>();
        ps = GetComponent<ParticleSystem>();
        icon_image.enabled = true;
        icon_button.enabled = true;
    }

    private void OnParticleCollision(GameObject other)
    {
        //ParticleSystem.Particle[] p = new ParticleSystem.Particle[1];
        //GetComponent<ParticleSystem>().GetParticles(p);
        
        if (spellType == 1 && other.GetComponent<Unlocks>())
        {
            other.GetComponent<Unlocks>().health -= damage;
            Debug.Log("Door Opened");
        }
        else if (spellType == 1 && other.GetComponent<Enemy>() && hits <= 2)
        {
            other.GetComponent<Enemy>().health -= damage;
            reset = true;
            hits++;
        }
        else if (spellType == 2 && other.GetComponent<Enemy>())
        {
            other.GetComponent<Enemy>().health -= damage;
        }
        //GetComponent<ParticleSystem>().SetParticles(p);
    }
    private void OnParticleTrigger()
    {
        int numEnter = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        for (int i = 0; i < numEnter; i++)
        {
            ParticleSystem.Particle p = enter[i];
            if (spellType == 2)
            {
                Debug.Log("take daamememege");
            }
            enter[i] = p;
        }

        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);

        Debug.Log("Working!");
    }


    // Update is called once per frame
    void Update()
    {
        if (hits > 2)
        {
            reset = false;
        }
        
        cooldown -= Time.deltaTime;

        if (cooldown >= 0f)// && !transform.parent.parent.gameObject.GetComponent<CharacterController>().bools[0])
        {
            icon_image.fillAmount = Mathf.InverseLerp(0f, cdTime, cooldown);
        }
        

        if (cooldown <= 0f)
        {
            hits = 0;
        }
        if ((isCast && cooldown <= 0) || (reset && isCast))
        {
            Cast();
            
            
        }
        isCast = false;

    }

    private void Cast()
    {
        gameObject.GetComponent<ParticleSystem>().Play();
        //gameObject.GetComponent<>
        //foreach (AudioSource s in sfx)
        //{
        //   s.Play();
        //}
        sfx[hits].Play();
        sfx[hits + 1].Play();
        isCast = false;
        reset = false;
        wand.Play("Base Layer.swingWand");
        cooldown = cdTime;
    }

    
}
