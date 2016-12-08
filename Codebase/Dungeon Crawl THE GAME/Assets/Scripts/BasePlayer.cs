using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public enum playerStates
{
    normal,
    throwing
}

public class BasePlayer : MonoBehaviour
{
    //Basic Settings
    int HP = 3;
    public playerStates currentState = playerStates.normal;
    public int numberOfJumps = 3;
    public bool canGrabTail = false;
    public float tossSpeed;
    public float tossSeconds = 1.5f;

    //Internal Basic Settings
    int currentJump = 3;
    bool tailGrabbed = false;
    bool burning = false;
    bool invulnerable = false;
    CharacterController controller;
    bool hasThrown = false;

    //Physics Settings
    public float speed = 6.0F;
    public float sprintSpeed = 10.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float mass = 20.0F;
    public float rotationSpeed = 2.0f;

    //Physics Internals
    Vector3 moveDirection = Vector3.zero;
    Vector3 Impact = Vector3.zero;

    //External References
    public Rigidbody marioRb;
    public Rigidbody wowserRb;
    public GameObject getTail;
    public GameObject Wowser;
    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;
    public GameObject BurnEffect;

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case playerStates.normal:
                if (controller.isGrounded)
                {
                    moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
                    moveDirection = transform.TransformDirection(moveDirection);
                }
                break;
            case playerStates.throwing:
                moveDirection = new Vector3(0, 0, 0);
                moveDirection = transform.TransformDirection(moveDirection);
                break;
            default:
                break;
        }

        if (controller.isGrounded)
        {
            if (Input.GetKey(KeyCode.LeftShift))
                moveDirection *= sprintSpeed;
            else
                moveDirection *= speed;

            currentJump = numberOfJumps;
        }

        if (Input.GetKeyDown("space") && currentJump > 0)
        {
            currentJump--;
            moveDirection.y = jumpSpeed;
        }

        if (Input.GetKeyDown(KeyCode.E) && canGrabTail)
        {
            if (tailGrabbed)
            {
                Wowser.GetComponent<Wowser>().CurrentState = BossStates.Idle;
                Wowser.transform.parent = null;
                tailGrabbed = false;

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !hasThrown)
                    StartCoroutine("tossTime");

                else
                {
                    Wowser.GetComponent<NavMeshAgent>().enabled = true;
                    Wowser.GetComponent<Wowser>().CurrentState = BossStates.Moving;
                }
                currentState = playerStates.normal;
            }
            else
            {

                Wowser.GetComponent<NavMeshAgent>().enabled = false;
                Wowser.GetComponent<Wowser>().CurrentState = BossStates.Idle;
                currentState = playerStates.throwing;
                Wowser.transform.parent = transform;
                Wowser.transform.rotation = transform.rotation;
                Wowser.transform.position = transform.position + transform.forward * 3f; ;

                tailGrabbed = true;
            }
        }
        if (Impact.magnitude > 0.2)
        {
            
            moveDirection = transform.TransformDirection(Impact);
            Impact = Vector3.Lerp(Impact, Vector3.zero, 5 * Time.deltaTime);
        }
         //consumes the impact energy each cycle: 
        moveDirection.y -= gravity * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);
        controller.Move(moveDirection * Time.deltaTime);

        if (transform.position.y < 0.7f)
        {
            transform.position = new Vector3(0, 10, 0);
            TakeDamage();
            StartCoroutine("Burning");
        }
    }

    public void TakeDamage()
    {
        if (!invulnerable)
        {
            switch (HP)
            {
                case 1:
                    //Life1.GetComponent<Renderer>().enabled = false;
                    if (HP == 3)
                    {
                        Life1.gameObject.SetActive(true);
                        Life2.gameObject.SetActive(true);
                        Life3.gameObject.SetActive(true);
                    }
                    
                    break;
                case 2:
                    //Life2.GetComponent<Renderer>().enabled = false;
                    if (HP == 2)
                    {
                        Life1.gameObject.SetActive(false);
                        Life2.gameObject.SetActive(true);
                        Life3.gameObject.SetActive(true);
                    }
                    break;
                case 3:
                    //Life3.GetComponent<Renderer>().enabled = false;
                    if (HP == 1)
                    {
                        Life1.gameObject.SetActive(false);
                        Life2.gameObject.SetActive(false);
                        Life3.gameObject.SetActive(true);
                    }
                    
                    break;
                case 4:
                    if (HP == 0)
                    {
                        Life1.gameObject.SetActive(false);
                        Life2.gameObject.SetActive(false);
                        Life3.gameObject.SetActive(false);
                    }
                    SceneManager.LoadScene("KillMario");
                    break;
            }
            HP--;
        }

        StartCoroutine("Invulnerable");
    }

    public void AddImpact(Vector3 direction, float force)
    {
        direction.Normalize();
        if (direction.y < 0)
            direction.y = -direction.y;
        Impact = direction.normalized * force / mass;
    }



    public void TakeFireDamage()
    {

        if (!burning)
        {
            TakeDamage();

            StartCoroutine("Burning");
        }
    }

    IEnumerator tossTime()
    {
        hasThrown = true;
        Wowser.GetComponent<Wowser>().CurrentState = BossStates.Idle;
        Wowser.GetComponent<Rigidbody>().isKinematic = false;
        Wowser.GetComponent<Rigidbody>().velocity = (transform.forward * tossSpeed);
        yield return new WaitForSeconds(tossSeconds);
        Wowser.GetComponent<Rigidbody>().isKinematic = true;
        Wowser.GetComponent<NavMeshAgent>().enabled = true;
        Wowser.GetComponent<Wowser>().CurrentState = BossStates.Moving;

        hasThrown = false;

        Wowser.GetComponent<Wowser>().CurrentState = BossStates.Moving;
    }

    IEnumerator Burning()
    {

        burning = true;
        //Generate Burn Flames
        GameObject Flames = (GameObject)Instantiate(BurnEffect, transform.position, new Quaternion(0, 45, 45, 0));
        //Attach to player
        Flames.transform.parent = transform;

        yield return new WaitForSeconds(2);
        Destroy(Flames);
        burning = false;

    }

    IEnumerator Invulnerable()
    {
        invulnerable = true;

        yield return new WaitForSeconds(1);

        invulnerable = false;
    }
}