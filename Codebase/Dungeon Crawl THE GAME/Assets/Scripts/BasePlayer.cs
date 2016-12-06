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
    public float tossSpeed;
    public float tossSeconds = 1.5f;

    //Internal Basic Settings
    int currentJump = 3;
    bool tailGrabbed = false;
    bool burning = false;
    bool invulnerable = false;
    bool hasThrown = false;
    CharacterController controller;



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

    //Public References
    public Rigidbody marioRb;
    public Rigidbody wowserRb;
    public GameObject getTail;
    public GameObject Wowser;
    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;
    public GameObject BurnEffect;

    //References
    Wowser WowserScript;


    // Use this for initialization
    void Start()
    {
        controller = GetComponent<CharacterController>();
        WowserScript = Wowser.GetComponent<Wowser>();
    }




    // Update is called once per frame
    void Update()
    {
        //if (canGrabTail == true)
        //{
        //    Debug.Log("CanGrabTail = true");
        //}

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

        if (Input.GetKeyDown(KeyCode.E) && WowserScript.canGrabTail)
        {
            if (tailGrabbed)
            {
               WowserScript.CurrentState = BossStates.Idle;
                Wowser.transform.parent = null;
                tailGrabbed = false;

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !hasThrown)
                {
                    StartCoroutine("tossTime");
                }
                else
                {
                    WowserScript.CurrentState = BossStates.Moving;
                }
                currentState = playerStates.normal;
            }
            else
            {

                WowserScript.CurrentState = BossStates.Idle;
                currentState = playerStates.throwing;
                //Move wowser to me;
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

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 10, 0);
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        if (!invulnerable)
        {
            switch (HP)
            {
                case 1:
                    Life1.GetComponent<Renderer>().enabled = false;
                    SceneManager.LoadScene("KillMario");
                    break;
                case 2:
                    Life2.GetComponent<Renderer>().enabled = false;
                    break;
                case 3:
                    Life3.GetComponent<Renderer>().enabled = false;
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
        WowserScript.CurrentState = BossStates.Idle;
        wowserRb.velocity = (transform.forward * tossSpeed);
        yield return new WaitForSeconds(tossSeconds);
        hasThrown = false;

        WowserScript.CurrentState = BossStates.Moving;


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