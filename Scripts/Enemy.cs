/* 
 * author : Alex Holdo
 */

using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    
    public float health = 3;
    public bool isAlive = true;
    private Vector3 location;
    public GameObject target;
    public Vector3 distance;
    public int runs = 0;

    public bool isRunning = false;

    public float jumpForce = 4f;
    public int numJumps = 1;
    private int jumpsLeft = 1;
    private Rigidbody rb;

    private Material mt;
    private Material mt2;
    [SerializeField]
    private Color mat;
    private Color mat3;
    private Color mat2;
    private Color mat4;

    [SerializeField]
    public float timer = 0;
    private float timer2 = 0f;

    public Animator sword;
    private Animator bounce;

    public float tim = 0;
    [SerializeField]
    private bool canAttack = true;

    public float swingTime = 2f;
    [Range(0f, 100f)]
    public float damage = 10;

    public int type = 1;

    public GameObject death;
    public GameObject sfx;

    public bool electrified;
    public float eTimer;

    public NavMeshAgent agent;
    public GameObject a;
    // Start is called before the first frame update
    void Start()
    {
        eTimer = 0;
        electrified = false;
        sword.gameObject.GetComponentInChildren<TrailRenderer>().emitting = false;
        bounce = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        mat = GetComponent<Renderer>().materials[0].color; 
        mat2 = GetComponent<Renderer>().materials[1].color;
        mat3 = mat;
        mat4 = new Color(mat.r, mat.g, mat.b, 0f);
        mt = GetComponent<Renderer>().materials[0];
        mt2 = GetComponent<Renderer>().materials[1];
    }

    public IEnumerator KillHim()
    {
        Debug.Log("running");
        sword.SetBool("canAtk", true);//play swinging animation
        sword.gameObject.GetComponentInChildren<TrailRenderer>().emitting = true;
        target.GetComponent<CharacterController>().UpdateHealth(-damage); //harm player
        agent.enabled = false;

        //swing cooldown
        yield return new WaitForSeconds(swingTime/5f);
        sword.gameObject.GetComponentInChildren<TrailRenderer>().emitting = false;
        yield return new WaitForSeconds(swingTime * 1.5f / 5f);
        agent.enabled = true;
        yield return new WaitForSeconds(swingTime/2f - .25f);
        

        sword.SetBool("canAtk", false);
        canAttack = false;
        

    }
    
    
    //[System.Obsolete]
    private void OnCollisionEnter(Collision collision)
    {
        agent.enabled = true;
    }
    
    
    public IEnumerator Bounce()
    {
        isRunning = true;
        jumpsLeft = numJumps;
        //bounce.Play("Base Layer.Bounce");
        Debug.Log("enemy jumped");
        jumpsLeft--;

        yield return new WaitForSeconds(1f);
        isRunning = false;
    }

    


    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        eTimer += Time.deltaTime;
        //timer2 += Time.deltaTime;

        sword.SetFloat("time", timer);

        if (electrified)
        {
            if (eTimer >= 5f)
            {
                electrified = false;
            }           
        }

        
        if (health <= 0f && tim < 4f && tim >= 3f)
        {

            GetComponent<Renderer>().materials[0].color = Color.Lerp(mat3, mat4, tim - 3f);
            GetComponent<Renderer>().materials[1].color = Color.Lerp(mat3, mat4, tim - 3f);
            Debug.Log("bruh");
            //mat2 = Color.Lerp(mat3, mat4, tim - 3f);
            tim += Time.deltaTime;
        }
        else if (health <= 0f && tim < 10f && tim >= 3f)
        {
            if(CompareTag("Boss"))
            {
                a.transform.position = transform.position;
                a.SetActive(true);
            }
            
            Destroy(gameObject);
        }
        else if (health <= 0f)
        {
            isAlive = false;
            rb.freezeRotation = false;
            tim += Time.deltaTime;

        }
        else
        {
            location = gameObject.transform.position;

            distance = target.transform.position - location;
            if (gameObject.CompareTag("Boss") && distance.magnitude < 100)
            {
                agent.SetDestination(target.transform.position);
            }
            else if (distance.magnitude < 10 && distance.magnitude >= 1 && Mathf.Abs(distance.y) < 2)
            {
                //.useGravity = false;
                agent.SetDestination(target.transform.position);
                //if (jumpsLeft > 0)
                {
                    //agent.enabled = false;
                    //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                    //Debug.Log("enemy jumped"); //code not working? but it runs?
                    //jumpsLeft--;
                    //agent.enabled = true;
                }
                if (!isRunning)
                {
                    StartCoroutine(Bounce());
                }
                
                timer2 += Time.deltaTime;
                //else if (timer2 < 1.5f && timer2 > 1f)
                //{
                //transform.parent.position = Vector3.Slerp(transform.parent.position, transform.parent.position + Vector3.up/2, (timer2 - 1f) * 2);
                //}
            }
            else if (distance.magnitude < 1 && timer > swingTime)
            {
                timer = 0f;
                sword.SetBool("canAtk", true);
                canAttack = true;
                StartCoroutine(KillHim());
                //bounce.SetBool("isChasing", false);
            }
            if (gameObject.CompareTag("Boss") && distance.magnitude < 2f && agent.enabled)
            {
                timer = 0f;
                //sword.SetBool("canAtk", true);
                canAttack = true;
                StartCoroutine(KillHim());
            }
            

        }
        if (isAlive == false)
        {
            StartCoroutine(RunOnce());
        }

    }



    private IEnumerator RunOnce()
    {
        if (runs == 0)
        {
            death.GetComponent<ParticleSystem>().Play();
            sfx.GetComponent<AudioSource>().Play();
            //gameObject.GetComponent<Renderer>().enabled = false;

            rb.AddForce(-distance * 120f);
            runs++;
            Destroy(agent);
            yield return new WaitForSecondsRealtime(3f);

            //GetComponent<Renderer>().enabled = false;
            
        }


    }
}
