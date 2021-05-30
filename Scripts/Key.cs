using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Key : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    private float timer;
    private bool active;
    private Color32 color1;
    private Color32 color2;
    // Start is called before the first frame update
    private void Start()
    {
        tmp.faceColor = new Color32(255, 255, 255, 0);
        timer = 0f;
        color1 = tmp.faceColor;
        color2 = new Color32(255, 255, 255, 255);
        active = false;
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer < .5f && active)
        {
            if (timer > .5f)
                timer = .5f;
            tmp.faceColor = Color32.Lerp(color1, color2, timer / .5f);
            tmp.rectTransform.position = new Vector3(tmp.rectTransform.position.x, Mathf.Lerp(tmp.rectTransform.position.y, tmp.rectTransform.position.y + 4f, timer/.5f), tmp.rectTransform.position.z);
        }
        else if (timer >= 1f && timer <= 3f && active)
        {
            tmp.faceColor = Color32.Lerp(color2, color1, (timer - 1.9f));
        }
        else if (active && timer > 3f)
        {
            tmp.faceColor = color1;
            gameObject.SetActive(false);
        }
     
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider>().enabled = false;
            Debug.Log("Picked Up Wooden Chest Key");
            timer = 0f;
            active = true;
            GetComponent<Renderer>().enabled = false;
            GetComponentInChildren<ParticleSystem>().Pause();
            GetComponentInChildren<AudioSource>().Play();
            GetComponentInChildren<Light>().enabled = false;       
        }
    }
}
