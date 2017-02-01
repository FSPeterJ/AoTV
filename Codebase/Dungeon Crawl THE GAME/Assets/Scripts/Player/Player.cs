using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public AudioClip deathSFX;
    //States

    enum States
    {
        Idle, MoveForward, Attack, SpinAttack, Die, TakeDamage, Teleport, LowPriorityIdle
    }
    States _cs;
    States currentState
    {
        get { return _cs; }
        set
        {

            if (_cs == States.SpinAttack)
            {
                anim.SetBool("Spin Attack", false);
                weaponScript.AttackEnd();
            }
            switch (value)
            {
                case States.Idle:
                    //You can prevent a state assignment with a check here
                    anim.SetBool("Move Forward", false);
                    _cs = value;
                    break;
                case States.MoveForward:
                    if (_cs == States.Idle)
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
                    anim.SetBool("Spin Attack", true);
                    weaponScript.AttackStart();
                    _cs = value;
                    break;
                case States.Die:
                    if (!dead)
                    {
                        anim.SetBool("Die", true);
                        dead = true;
                        EventSystem.PlayerDeath();
                        _cs = value;
                    }
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

    bool _tT = false;
    bool teleportToggle
    {
        get
        {
            return _tT;
        }
        set
        {
            if (_tT != value)
            {
                _tT = value;
                if (_tT)
                {
                    tpMarker = Instantiate(teleportMarker);
                    tpMarker.transform.parent = transform;
                    tpMarker.transform.position = transform.position;
                }
                else
                {
                    tpDestination = tpMarker.transform.position;
                    Destroy(tpMarker);
                    anim.SetTrigger("Teleport");

                }
            }
        }
    }


    //Basic Settings - Edit in Unity
    public int maxJump = 1;

    public float movementModfier = .75f;
    public int health = 30;
    public int lives = 3;

    public HUD Hud;

    //Variables
    bool invulnerable = false;
    bool burning = false;
    int maxJumpStored;

    //Component References
    Animator anim;
    CharacterController controller;
    //This is a hack together way to get the weapon.
    public GameObject weapon;
    IWeaponBehavior weaponScript;
    public GameObject teleportMarker;
    GameObject tpMarker;
    Vector3 tpDestination;


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
    bool dead = false;

    //Control Settings
    Vector3 mousePosition;
    float mouseDistance;



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
        if (!dead)
        {
            Hud.PrintScore();
            //Re-used a lot of Harrison's movement code
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            mouseDistance = Vector3.Distance(mousePosition, transform.position);
            //Alternate Control Scheme - bad imo
            //moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= sprintSpeed;
            moveDirection *= speed;



            if (currentState == States.Idle || currentState == States.MoveForward)
            {
                if (Input.GetMouseButton(0))
                {
                    currentState = States.Attack;

                }
                else if (Input.GetMouseButton(1))
                {
                    currentState = States.SpinAttack;

                }
            }



            if (currentState == States.SpinAttack)
            {
                if (!Input.GetMouseButton(1))
                {
                    currentState = States.Idle;
                    weaponScript.AttackEnd();
                }
            }
            else
            {
                //Strafe and reverse modified with multiplier
                var localVel = transform.InverseTransformDirection(moveDirection);
                if (localVel.z > 0 && localVel.z > localVel.x)
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


                if (Impact.magnitude > 0.2)
                {
                    moveDirection += Impact;
                    Impact = Vector3.Lerp(Impact, Vector3.zero, 5 * Time.deltaTime);
                }


                //The character still twitches a bit in very specific positions due to his vertical bobbing
                if (mouseDistance > 1.9f)
                {

                    //Turn player to face cursor on terrain
                    Vector3 lookPos = (transform.position - mousePosition);
                    lookPos.y = 0;
                    float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) - 90, .2f);
                    //float angle = -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) - 90;
                    transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
                }

            }


            // ^^^ Probably could be done better than this.
            // Agreed

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

            if (!teleportToggle && Input.GetKey(KeyCode.Q))
            {
                teleportToggle = true;
            }
            else if (teleportToggle && !(Input.GetKey(KeyCode.Q)))
            {
                teleportToggle = false;
            }

            if (teleportToggle)
            {
                //tpMarker.transform.rotation = transform.rotation;
                float md = (mouseDistance < 40) ? mouseDistance / 2 : 20;
                tpMarker.transform.localPosition = new Vector3(0, mousePosition.y + .2f, md);

            }



            //Gravity
            verticalVel += gravity * Time.deltaTime;
            moveDirection.y -= verticalVel;



            //Move
            controller.Move(moveDirection * Time.deltaTime);
            EventSystem.PlayerPositionUpdate(transform.position);

        }

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
            Hud.UpdateHealth(health);
            Debug.Log("health = " + Hud.healthslider.value);

            if (health < 1)
            {
                GetComponent<AudioSource>().PlayOneShot(deathSFX);
                currentState = States.Die;
            }
            else
            {
                GetComponent<AudioSource>().Play();
            }
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

    IEnumerator Invulneraball()
    {
        invulnerable = true;
        yield return new WaitForSeconds(10);
        invulnerable = false;
    }


    public void AttackFinished(int attack)
    {
        if (currentState == States.Attack)
        {

            if (attack == 1)
                anim.SetBool("Slash Attack 01", false);
            else
                anim.SetBool("Slash Attack 02", false);

            currentState = States.Idle;
            weaponScript.AttackEnd();
        }
    }

    public void ResetAttackStack()
    {
        weaponScript.ResetAttack();
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Trapdoor")
            if (Input.GetKeyDown(KeyCode.E))
                SceneManager.LoadScene("Graveyard");

        if (col.tag == "LastDoor")
            SceneManager.LoadScene("Graveyard");

    }
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ScorePowerUp")
        {

            Destroy(col.gameObject);
            //waiting on fixxed score system 
        }
        else if (col.tag == "HealthPowerUp")
        {
            Destroy(col.gameObject);
            health = 10;
            Hud.UpdateHealth(health);
        }
        else if (col.tag == "Trapdoor")
            col.gameObject.GetComponent<Animator>().SetBool("Close", false);
 
        else if (col.tag == "Invulneraball")
        {
            Invulneraball();
            Destroy(col.gameObject);
        }
        else if (col.tag == "Health Collectible")
        {
            Destroy(col.gameObject);
            health = health + 3;
            Hud.UpdateHealth(health);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Trapdoor")
            col.gameObject.GetComponent<Animator>().SetBool("Close", true);
    }

    void TeleportMove()
    {
        transform.position = tpDestination;
    }


}