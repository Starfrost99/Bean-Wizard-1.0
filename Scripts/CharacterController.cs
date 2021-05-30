/* 
 * author : Alex Holdo
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {

    public float speed = 5.0f;
    public float sprintSpeed = 10f;
    private float translation;
    private float straffe;
    [SerializeField]
    private float health = 100f;
    public Slider healthSlider;

    public Transform spawnPoint;
    public Transform spawnPoint1;

    public bool isAlive = true;
    private int runs = 0;
    public float jumpForce = 5f;
    public int numJumps = 1;
    private int jumpsLeft = 1;
    private Rigidbody rb;

    public GameObject[] inventory;
    public int activeItem = 0;
    public GameObject spellbook;
    public Collider[] tomeTriggers;
    public bool[] bools;
    public GameObject tome2;
    public GameObject pauseMenu;
    private bool stunned;
    public GameObject mainCamera;

    public GameObject prefab;

    public GameObject go;

    private bool cd = false;
    // Use this for initialization
    void Start () {
        stunned = false;
        //Time.timeScale = 0.1f;
        // turn off the cursor
        Cursor.lockState = CursorLockMode.Locked;
        inventory = new GameObject[5];

        rb = GetComponent<Rigidbody>();

        bools = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            bools[i] = true;
        }

        activeItem = 0;
        //inventory[0] = spellbook.gameObject.GetComponent<Spells>().magicBolt;
        }
	// Update is called once per frame

    void CastSpell(GameObject spellToCast)
    {
        spellToCast.GetComponent<Spell>().isCast = true;
    }

    public void AddSpell(int spell)
    {
        switch (spell)
        {
            case 1:
                inventory[0] = spellbook.GetComponent<Spells>().magicBolt;
                break;
            case 2:
                inventory[1] = spellbook.GetComponent<Spells>().chainBolt;
                break;
        }
    }

    public void UpdateHealth(float change)
    {
        health += change;
        if (health < 0f)
            health = 0f;
        healthSlider.value = health;
    }
    private void UpdateHealth()
    {
        if (health > 100f)
        {
            health = 100f;
        }
        else if (health <= 0f)
        {
            isAlive = false;
            runs++;
            go.SetActive(true);
        }
    }
    void Update ()
    {
        

        UpdateHealth();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (pauseMenu.activeSelf)
            {
                case true: pauseMenu.SetActive(false); Time.timeScale = 1f; stunned = false; 
                    mainCamera.GetComponent<MouseCamLook>().enabled = true; break;

                case false: pauseMenu.SetActive(true); Time.timeScale = 0f; stunned = true; 
                    mainCamera.GetComponent<MouseCamLook>().enabled = false; break; 
            }
        }
            
        if (isAlive && !stunned)
        {
            GetInput();
        }
        else if (runs == 0)
        {
            GameOver();
        }

        if (Input.GetKeyDown("escape")) {
            //turn on the cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void GameOver()
    {
        //End Game
        Debug.Log("Game Over");
    }

    public IEnumerator Despawn()
    {
        prefab.SetActive(true);
        cd = true;
        yield return new WaitForSeconds(3f);
        cd = false;
        prefab.SetActive(false);
    }

    void GetInput()
    {
        if (Input.GetButton("Q Spell") && inventory[0] != null)
        {
            CastSpell(inventory[0]);
        }

        if (Input.GetButton("E Spell") && !bools[1])
        {
            //WIP chain lightning
            if (!cd)
            {
                StartCoroutine(Despawn());
                CastSpell(inventory[1]);
            }
                
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (jumpsLeft > 0)
            {
                if(jumpsLeft < numJumps + 1)
                    rb.AddForce(Vector3.up * jumpForce * 1.5f, ForceMode.Impulse);
                else
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

                jumpsLeft--;
            }
         
        }
        float speedModifier = 1;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedModifier = sprintSpeed;
        }
        else
        {
            speedModifier = speed;
        }
        // Input.GetAxis() is used to get the user's input
       
        translation = Input.GetAxis("Vertical") * speedModifier * Time.deltaTime;
        straffe = Input.GetAxis("Horizontal") * speedModifier * Time.deltaTime;
        transform.Translate(straffe, 0, translation);
    }

    //[System.Obsolete]
    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("test");
        if ((other.gameObject.CompareTag("Tome")) && bools[0])
        {
            other.gameObject.GetComponent<Renderer>().enabled = false;
            other.gameObject.transform.Find("Particles").gameObject.SetActive(false);
            other.gameObject.GetComponent<Tome>().Collect();
            bools[0] = false;

            AddSpell(1);
        }
        else if (other == tomeTriggers[1] && bools[1])
        { 
            other.gameObject.GetComponent<Tome>().Collect();
            bools[1] = false;
            AddSpell(2);
        }
        else if (other == tomeTriggers[2] && bools[2])
        {
            other.gameObject.GetComponent<Tome>().Collect();
            bools[2] = false;
            AddSpell(3);
        }
        else if (other == tomeTriggers[3] && bools[3])
        {
            other.gameObject.GetComponent<Tome>().Collect();
            bools[3] = false;
            AddSpell(4);
        }
        else if (other == tomeTriggers[4] && bools[4])
        {
            other.gameObject.GetComponent<Tome>().Collect();
            bools[4] = false;
            AddSpell(5);
        }
        if (other.gameObject.CompareTag("bt"))
        {
            transform.position = spawnPoint1.position;
        }
        if (other.gameObject.GetComponent<Heart>().active)
        {
            health = 100f;
            healthSlider.value = health;
            Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.collider.gameObject.CompareTag("Kill"))
        {
            transform.position = spawnPoint.position;
        }
        if (bools[1] && tome2.activeSelf)
        {
            tome2.GetComponent<Tome>().Collect();
            bools[1] = false;
            AddSpell(2);
        }
        if (collision.collider.gameObject.CompareTag("Floor"))
            jumpsLeft = numJumps + 1;
    }
    
}