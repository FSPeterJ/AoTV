using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //States

    enum States
    {
        Idle, MoveForward, Attack, SpinAttack, Dying, TakeDamage, Teleport, LowPriorityIdle
    }
    States _cs;
    States currentState
    {
        get { return _cs; }
        set
        {
            switch (value)
            {
                case States.Idle:
                    //You can prevent a state assignment with a check here
                    anim.SetBool("Move Forward", false);
                    _cs = value;
                    break;
                case States.MoveForward:
                    if(_cs == States.Idle)
                    {
                        anim.SetBool("Move Forward", true);
                        _cs = value;
                    }
                    break;
                case States.Attack:
                    anim.SetBool("Slash Attack 01", true);
                    weaponScript.AttackStart();
                    _cs = value;
                    break;
                case States.SpinAttack:
                    anim.SetBool("SpinAttack", true);
                    _cs = value;
                    break;
                case States.Dying:
                    anim.SetBool("Die", true);
                    _cs = value;
                    break;
                case States.TakeDamage:
                    anim.SetBool("TakeDamage", true);
                    _cs = value;
                    break;
                case States.Teleport:
                    anim.SetBool("Teleport", true);
                    _cs = value;
                    break;
                case States.LowPriorityIdle:
                    if (_cs == States.MoveForward)
                    {
                        currentState = States.Idle;
                    }
                    break;
                default:
                    _cs = value;
                    break;
            }
        }
    }


    //Basic Settings - Edit in Unity
    public int maxJump = 1;
    int maxJumpStored;
    public float movementModfier = .75f;
    public int health = 3;
    bool invulnerable = false;
    bool burning = false;

    //Component References
    Animator anim;
    CharacterController controller;
    //This is a temporary hack.
    public GameObject weapon;
    IWeaponBehavior weaponScript;

    //Physics Settings
    public float speed = 3.0F;
    public float sprintSpeed = 6.0f;
    public float jumpSpeed = 10.0F;
    public float gravity = 9.8F;
    public float mass = 20.0F;
    public float rotationSpeed = 5.0f;

    //Physics Internals
    Vector3 moveDirection = Vector3.zero;
    Vector3 Impact = Vector3.zero;
    float verticalVel = 0;

    //Control Settings
    private Vector3 mousePosition;
    private Vector3 direction;


    void OnEnable()
    {
        EventSystem.onMousePositionUpdate += UpdateMousePosition;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onMousePositionUpdate -= UpdateMousePosition;
    }


    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        maxJumpStored = maxJump;
        currentState = States.Idle;
        weaponScript = weapon.GetComponent<IWeaponBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        //Re-used a lot of Harrison's movement code
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //Alternate Control Scheme - bad imo
        //moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= sprintSpeed;
        moveDirection *= speed;


        //Strafe and reverse modified with multiplier
        var localVel = transform.InverseTransformDirection(moveDirection);
        if(localVel.z > 0 && localVel.z > localVel.x)
        {
            currentState = States.MoveForward;
        }
        else 
        {
            currentState = States.LowPriorityIdle;
            if (localVel.z < 0)
            {
                localVel.z = localVel.z * movementModfier;
            }
        }
        localVel.x = localVel.x * movementModfier;
        moveDirection = transform.TransformDirection(localVel);
        // ^^^ Probably could be done better than this.

        //Landed / Grounded
        if (controller.isGrounded)
        {
            //anim.SetBool("Jump", false);
            verticalVel = 0; 
            maxJump = maxJumpStored;
        }

        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && maxJump > 0)
        {
            maxJump--;
            verticalVel -= jumpSpeed;
        }

        if(currentState == States.Idle || currentState == States.MoveForward)
        {
            if (Input.GetMouseButton(0))
            {
                currentState = States.Attack;

            }

        }

        //Gravity
        verticalVel += gravity * Time.deltaTime;
        moveDirection.y -= verticalVel;

        //Turn player to face cursor on terrain
        Vector3 lookPos = (transform.position - mousePosition);
        float angle = -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) - 90;
        transform.rotation = Quaternion.AngleAxis(angle, new Vector3( 0,1,0));

        //Move
        controller.Move(moveDirection * Time.deltaTime);

        //Tell subscribers the player has moved
        EventSystem.PlayerPositionUpdate(transform.position);
    }

    void UpdateMousePosition(Vector3 MousePos)
    {
        mousePosition = MousePos;
    }


    public void TakeDamage(int dmg = 1)
    {
        if (!invulnerable)
        {
            StartCoroutine("Invulnerable");
            EventSystem.PlayerHealthUpdate(-dmg);
            health--;
        }
    }
    //Use this to send the character flying with a force from a given direction
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


    IEnumerator Burning()
    {

        burning = true;
        //Generate Burn Flames
        //GameObject Flames = (GameObject)Instantiate(BurnEffect, transform.position, new Quaternion(0, 45, 45, 0));
        //Attach to player
        //Flames.transform.parent = transform;

        yield return new WaitForSeconds(2);
        //Destroy(Flames);
        burning = false;

    }

    IEnumerator Invulnerable()
    {
        invulnerable = true;

        yield return new WaitForSeconds(1);

        invulnerable = false;
    }




    public void AttackFinished(int attack)
    {
        if(currentState == States.Attack)
        {

            if(attack == 1)
            {
                anim.SetBool("Slash Attack 01", false);
                
            }
            else
            {
                anim.SetBool("Slash Attack 02", false);
                
            }
            currentState = States.Idle;
            weaponScript.AttackEnd();
        }
    }





}


