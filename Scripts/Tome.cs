/* 
 * author : Alex Holdo
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Tome : MonoBehaviour
{
    public string id = "Tome_Bolt";
    public AudioSource sfx;
    public GameObject tomeFX;
    public GameObject tomeFX1;
    public Material zeroVignette;
    public Material oneVignette;
    private float timer = 999f;

    public TextMeshProUGUI tmp;
    public TextMeshProUGUI tmp2;

    void Start()
    {
        tmp.faceColor = new Color(255f, 255f, 255f, 0f);
        tmp2.faceColor = new Color(255f, 255f, 255f, 0f);
    }

    public AudioSource ambient;
    // Start is called before the first frame update
    public void Collect()
    {
        if (id == "Tome_Force")
            sfx.Play(1);
        else
        {
            sfx.Play();
        }

        
        //tomeFX.GetComponent<Animator>().SetBool("triggered", true);
        if (id == "Tome_Force")
        {
            //gameObject.GetComponent<Renderer>().enabled = false;
            //gameObject.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);
        }
        
        //Destroy(ambient.gameObject);
        //Debug.Log("hi");
        StartCoroutine(Co());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        //Debug.Log(timer);
        //START TOME 2
        if (timer >= 0f && timer <= .5f && id == "Tome_Force")
        {
            tomeFX.GetComponent<Renderer>().material.Lerp(zeroVignette, oneVignette, Mathf.InverseLerp(0f, .5f, timer));
        }
        else if (timer >= 2f && timer <= 3f && id == "Tome_Force")
        {
            tomeFX.GetComponent<Renderer>().material.Lerp(oneVignette, zeroVignette, Mathf.InverseLerp(2f, 2.8f, timer));
        }
        if (timer >= 0f && timer <= .5f && id == "Tome_Force")
        {
            tmp.faceColor = new Color32(255, 255, 255, (byte)Mathf.Lerp(0f, 255f, timer / .5f));
            //Debug.Log(tmp.alpha);
        }
        else if (timer >= 2f && timer <= 3f && id == "Tome_Force")
        {
            tmp.faceColor = new Color32(255, 255, 255, (byte)Mathf.Lerp(255f, 0f, (timer - 2f)));
        }
        else
            tmp.faceColor = new Color32(255, 255, 255, 0);

        // START TOME 1
        if (timer >= 0f && timer <= .5f && id == "Tome_Bolt")
        {         
            tomeFX.GetComponent<Renderer>().material.Lerp(zeroVignette, oneVignette, Mathf.InverseLerp(0f, .5f, timer));
        }
        else if (timer >= 5f && timer <= 7f && id == "Tome_Bolt")
        {
            tomeFX.GetComponent<Renderer>().material.Lerp(oneVignette, zeroVignette, Mathf.InverseLerp(5f, 7f, timer));
        }
        if (timer >= 0f && timer <= 2f && id == "Tome_Bolt")
        {
            tmp.faceColor = new Color32(255, 255, 255, (byte)Mathf.Lerp(0f, 255f, timer / 2f));
            //Debug.Log(tmp.alpha);
        }
        else if (timer >= 5f && timer <= 8f && id == "Tome_Bolt")
        {
            tmp.faceColor = new Color32(255, 255, 255, (byte)Mathf.Lerp(255f, 0f, (timer-5f) / 2f));
            tmp2.faceColor = new Color32(255, 255, 255, (byte)Mathf.Lerp(255f, 0f, (timer - 5f) / 2f));
        }

        if (timer >= 1f && timer <= 3f && id == "Tome_Bolt")
        {
            tmp2.faceColor = new Color32(255, 255, 255, (byte)Mathf.Lerp(0f, 255f, (timer - 1f) / 2f));
            //Debug.Log(tmp.alpha);
        }
    }

    public IEnumerator Co()
    {
        if (id == "Tome_Bolt")
        {
            tomeFX.SetActive(true);
            timer = 0f;
            ambient.enabled = false;

            yield return new WaitForSeconds(9f);
            tomeFX.SetActive(false);
        }       
        else if (id == "Tome_Force")
        {
            timer = 0f;
            yield return new WaitForSeconds(4f);
            tomeFX1.SetActive(false);
        }
            
        

    }

}
