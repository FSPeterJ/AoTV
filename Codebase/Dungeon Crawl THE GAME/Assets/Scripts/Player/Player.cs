using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField]
    AudioClip deathSFX;
    [SerializeField]
    GameObject LossScreen, WinScreen;


    //Basic Settings - Edit in Unity

    [SerializeField]
    int maxJump = 1;
    [SerializeField]
    float strafeModfier = .75f;
    [SerializeField]
    int health = 30;
    [SerializeField]
    int healthMax = 30;
    [SerializeField]
    uint lives = 3;
    [SerializeField]
    float scaleFactor = 2;

    //This is not allowed.
    //[SerializeField] HUD Hud;

    //Variables
    bool invulnerable = false;
    bool burning = false;
    int maxJumpStored;


    //Component References
    Animator anim;
    CharacterController controller;
    GameObject weapon;
    IWeaponBehavior weaponScript;
    GameObject teleportMarker;
    GameObject tpMarker;
    Vector3 tpDestination;
    GameObject rangedScythe;
    GameObject rangedScytheAttack;

    //Physics Settings
    float gravity = 9.8F;
    [SerializeField]
    float speed = 3.0F;
    [SerializeField]
    float sprintSpeed = 6.0f;
    [SerializeField]
    float jumpSpeed = 10.0F;
    [SerializeField]
    float mass = 20.0F;
    [SerializeField]
    float rotationSpeed = 5.0f;

    //Physics Internals
    Vector3 moveDirection = Vector3.zero;
    Vector3 Impact = Vector3.zero;
    [SerializeField]
    float verticalVel = 0;
    [SerializeField]
    float verticalAccel = 0;
    bool dead = false;

    //Control Settings
    Vector3 mousePosition;
    float mouseDistance;
    Rigidbody rBody;
    float verticalInput;
    float horizontalInput;
    float jumpTime;
    [SerializeField]
    bool isgrounded = false;

    //Checkpoints
    Vector3 CurrentCheckpoint;

    //Spin Attack CD
    float spinTime = 999;
    float maxSpinTime = 3;
    float spinCDMax = 8;
    float spinCD = 999;

    //Teleport CD
    float teleportCDMax = 8;
    float teleportCD = 999;



    //Ranged attack
    float rangedAttackTime = 0;
    float rangedAttackCDMax = 8;
    float rangedAttackCD = 999;




    void OnEnable()
    {
        EventSystem.onPlayerGrounded += IsGrounded;
        EventSystem.onMousePositionUpdate += UpdateMousePosition;
        EventSystem.onPlayer_ReloadCheckpoint += ReloadCheckpoint;
        EventSystem.onPlayerScale += ScaleFactor;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerGrounded -= IsGrounded;
        EventSystem.onMousePositionUpdate -= UpdateMousePosition;
        EventSystem.onPlayer_ReloadCheckpoint -= ReloadCheckpoint;
        EventSystem.onPlayerScale -= ScaleFactor;
    }


    //States
    enum States
    {
        Idle, MoveForward, Attack, SpinAttack, Die, TakeDamage, Teleport, LowPriorityIdle, ThrowScythe
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
                    spinTime = 0;
                    _cs = value;
                    break;
                case States.Die:
                    if (!dead && lives < 1)
                    {
                        GetComponent<AudioSource>().PlayOneShot(deathSFX);
                        anim.SetBool("Die", true);
                        dead = true;
                        EventSystem.PlayerDeath();
                        EventSystem.LivesCount(lives);
                        EventSystem.PlayerLose();
                        _cs = value;
                    }
                    else
                    {
                        lives--;
                        currentState = States.Idle;
                        health = healthMax;
                        ReturnToCheckpoint();
                    }
                    break;
                case States.TakeDamage:
                    GetComponent<AudioSource>().Play();
                    weaponScript.AttackEnd();
                    anim.SetBool("Take Damage", true);
                    _cs = value;
                    break;
                case States.Teleport:
                    tpDestination = tpMarker.transform.position;
                    Destroy(tpMarker);
                    anim.SetTrigger("Teleport");
                    teleportCD = 0;
                    //_cs = value;
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
                
                if (value)
                {
                    tpMarker = Instantiate(teleportMarker);
                    tpMarker.transform.parent = transform;
                    tpMarker.transform.position = transform.position;
                    _tT = value;
                }
                else
                {
                    currentState = States.Teleport;
                    _tT = value;
                }
            }
        }
    }
    bool _tS = false;
    bool throwScythe
    {
        get
        {
            return _tS;
        }
        set
        {
            if (_tS != value)
            {

                if (value)
                {
                    if (rangedAttackCD > rangedAttackCDMax)
                    {
                        rangedAttackCD = 0;
                        rangedAttackTime = 0;
                        weapon.SetActive(false);
                        rangedScytheAttack = Instantiate(rangedScythe);
                        rangedScytheAttack.transform.position = transform.position + transform.up * scaleFactor;
                        rangedScytheAttack.transform.rotation = transform.rotation;
                        rangedScytheAttack.GetComponent<IRangedPlayerAttack>().ScaleFactor(scaleFactor);
                        _tS = value;
                    }
                }
                else
                {
                    Destroy(rangedScytheAttack);
                    weapon.SetActive(true);
                    _tS = value;
                }

            }
        }
    }


    // Use this for initialization
    void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        EventSystem.SpinTime(spinTime, maxSpinTime);
        anim = GetComponent<Animator>();
        teleportMarker = (GameObject)Resources.Load("Prefabs/Particles/TeleportTarget");
        rangedScythe = (GameObject)Resources.Load("Prefabs/Player/ScytheRangedAttack");
        maxJumpStored = maxJump;
        currentState = States.Idle;
        weapon = FindWeapon(transform);
        weaponScript = weapon.GetComponent<IWeaponBehavior>();
        rBody = GetComponent<Rigidbody>();
        if(KeyManager.GetKeyCode("Space") == KeyCode.None)
            KeyManager.SetKey("Space", KeyCode.Space);
        EventSystem.LivesCount(lives);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            EventSystem.GamePausedToggle();
        }
        if (!dead)
        {
            GetInput();
            spinCD += Time.deltaTime;
            teleportCD += Time.deltaTime;
            rangedAttackCD += Time.deltaTime;
            EventSystem.SpinCooldown(spinCD, spinCDMax);
            EventSystem.TeleportCooldown(teleportCD, teleportCDMax);
            EventSystem.RangedCooldown(rangedAttackCD, rangedAttackCDMax);

            Vector3 tempMousePostition = mousePosition;
            tempMousePostition.y = transform.position.y;
            mouseDistance = Vector3.Distance(tempMousePostition, transform.position);

            //Re-used a lot of Harrison's movement code

            if (!throwScythe && (currentState == States.Idle || currentState == States.MoveForward))
            {
                if (Input.GetButton("Fire2"))
                {
                    currentState = States.Attack;

                }
                else if (Input.GetButton("Fire3") && spinCD > spinCDMax)
                {
                    currentState = States.SpinAttack;
                }
                else if (Input.GetButton("Fire1"))
                {
                    throwScythe = true;
                }
            }

            if (currentState == States.SpinAttack)
            {
                if (!Input.GetButton("Fire3") || spinTime > maxSpinTime)
                {
                    spinCD = 0;
                    currentState = States.Idle;
                    weaponScript.AttackEnd();
                    spinTime = 99;
                }
                spinTime += Time.deltaTime;
                EventSystem.SpinTime(spinTime, maxSpinTime);
            }
            else
            {
                if (throwScythe)
                {
                    if (rangedAttackTime > 1.5)
                    {
                        if (Vector3.Distance(transform.position, rangedScytheAttack.transform.position) < 2.5 * scaleFactor)
                        {
                            throwScythe = false;
                        }
                    }
                    rangedAttackTime += Time.deltaTime;
                }

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
                        localVel.z = localVel.z * strafeModfier;
                    }
                }
                localVel.x = localVel.x * strafeModfier;
                moveDirection = transform.TransformDirection(localVel);





                //The character still twitches a bit in very specific positions due to his vertical bobbing
                if (mouseDistance > .25f * scaleFactor)
                {
                    //Turn player to face cursor on terrain
                    RotateToFaceTarget(mousePosition, rotationSpeed);
                }

            }

            //Landed / Grounded


            //Jump
            if (Input.GetButton("Jump") && maxJump > 0)
            {
                maxJump--;
                verticalVel += jumpSpeed * scaleFactor;
                jumpTime = 0;
            }

            if (teleportCD > teleportCDMax && !teleportToggle && Input.GetButton("Teleport"))
            {
                teleportToggle = true;
            }
            else if (teleportToggle && !(Input.GetButton("Teleport")))
            {
                teleportToggle = false;
            }

            if (teleportToggle)
            {
                //tpMarker.transform.rotation = transform.rotation;
                float md = (mouseDistance/ scaleFactor < 15 * scaleFactor) ? mouseDistance/ scaleFactor : 15 * scaleFactor;
                Debug.Log(mousePosition.y);
                Debug.Log(transform.position.y);
                tpMarker.transform.localPosition = new Vector3(0, mousePosition.y/scaleFactor + .1f* scaleFactor, md);
            }

            //Move
            EventSystem.PlayerPositionUpdate(transform.position);

        }

    }

    void FixedUpdate()
    {
        Move();
    }
     bool landed;
    //Calculate Physics movement
    void Move()
    {
        jumpTime+= Time.fixedDeltaTime;

        if (isgrounded  && jumpTime > 1f)
        {
            maxJump = maxJumpStored;
            verticalAccel = 0;
            verticalVel = 0;
            landed = true;
        }
        else
        {
            verticalAccel -= gravity * Time.fixedDeltaTime * scaleFactor;
        }
        moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection *= speed * scaleFactor;
        verticalVel += verticalAccel * Time.fixedDeltaTime;

        moveDirection.y += verticalVel;
        if (Impact.magnitude > 0.2 * scaleFactor)
        {
            moveDirection += Impact;
            Impact = Vector3.Lerp(Impact, Vector3.zero, 5 * Time.fixedDeltaTime);
        }
        rBody.velocity = moveDirection;
        
    }

    void GetInput()
    {
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
    }

    void ForcePush(Vector3 direction)
    {
        Vector3 norm = gameObject.transform.position - direction;
        norm.Normalize();
        if (norm.y < 0)
            norm.y = -norm.y;
        Impact = norm.normalized * 1000 / mass;
    }
    void IsGrounded(bool grounded)
    {
        isgrounded = grounded;


    }
    public void ResetToIdle()
    {
        currentState = States.Idle;
    }

    void UpdateMousePosition(Vector3 MousePos)
    {
        mousePosition = MousePos;
    }


    public void TakeDamage(int dmg = 1)
    {
        if (!invulnerable)
        {
            StartCoroutine(Invulnerable());

            health-= dmg;
            EventSystem.PlayerHealthUpdate(health, healthMax);
            if (health < 1)
            {
                currentState = States.Die;
                
                //Time.timeScale = 0;
            }
            else
                currentState = States.TakeDamage;
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

    IEnumerator Invulnerable(int seconds = 1)
    {
        invulnerable = true;

        yield return new WaitForSeconds(seconds);
            
        invulnerable = false;
    }

    //IEnumerator Shiny(int secs = 1)
    //{
    //    particle.transform.position = transform.parent.position;

    //    yield return new WaitForSeconds(secs);

    //    Destroy(particle, 10);
    //}

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
            if (Input.GetButton("Use"))
            {
                //gameObject.transform.localScale = new Vector3(2, 2, 2);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        if (col.tag == "ForestEnd")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (col.tag == "SwampEnd")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }



    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "ScorePowerUp")
        {
            Destroy(col.gameObject);
            EventSystem.IncScore(5);
            //waiting on fixxed score system 
        }
        else if (col.tag =="LifePowerUp")
        {
            lives += 1;
            EventSystem.LivesCount(lives);
            Destroy(col.gameObject);

            //waiting on fixxed score system
        }
        else if (col.tag == "HealthPowerUp")
        {
            Destroy(col.gameObject);
            health = healthMax;
            EventSystem.PlayerHealthUpdate(health, healthMax);
        }
        else if (col.tag == "Trapdoor")
            col.gameObject.GetComponent<Animator>().SetBool("Close", false);

        else if (col.tag == "InvunerablePowerUp")
        {
            StartCoroutine(Invulnerable(10));
            Destroy(col.gameObject);
        }
        else if (col.tag == "Checkpoint")
        {
            if (CurrentCheckpoint != col.transform.position)
            {
                CurrentCheckpoint.x = col.transform.position.x;
                CurrentCheckpoint.y = col.transform.position.y + 3;
                CurrentCheckpoint.z = col.transform.position.z;


                GameObject glow = col.transform.Find("Ground Glow").gameObject;

                glow.SetActive(true);
                // GameObject temp = GameObject.Find("Ground Glow");
                //temp.SetActive(true);

                //GameObject[] terribleStuff;
                //terribleStuff = col.GetComponentsInChildren<GameObject>();
                //foreach (GameObject GO in terribleStuff)
                //    if (GO.name == "Ground Glow")
                //        GO.SetActive(true);



                // GameObject.Find("Ground Glow").SetActive(true);




                // col.GetComponentInChildren<ParticleSystem>().Play();
            }
        }
        else if (col.tag == "OutOfBounds")
        {
            TakeDamage(100);
            ReturnToCheckpoint();
        }
        else if (col.tag == "WinArea")
        {
            WinScreen.SetActive(true);
        }
    }

    void ReturnToCheckpoint()
    {
        anim.SetBool("Teleport Appear", true);
        transform.position = CurrentCheckpoint;
        throwScythe = false;
        EventSystem.PlayerHealthUpdate(health, healthMax);
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

    GameObject FindWeapon(Transform obj)
    {
        foreach (Transform tr in obj)
        {
            if (tr.tag == "Weapon")
            {
                return tr.gameObject;
            }
            if (tr.childCount > 0)
            {
                GameObject temp = FindWeapon(tr);
                if (temp)
                {
                    return temp;
                }
            }
        }
        return null;
    }

    void RotateToFaceTarget(Vector3 _TargetPosition, float _LerpSpeed = 4f, float _AngleAdjustment = -90f)
    {
        Vector3 lookPos = (transform.position - _TargetPosition);
        lookPos.y = 0;
        float angle = Mathf.LerpAngle(transform.rotation.eulerAngles.y, -(Mathf.Atan2(lookPos.z, lookPos.x) * Mathf.Rad2Deg) + _AngleAdjustment, Time.deltaTime * _LerpSpeed);
        transform.rotation = Quaternion.AngleAxis(angle, new Vector3(0, 1, 0));
    }

    public void Continue()
    {
        lives++;
        health = 3;

        //Score -= Score >> 1;
        Time.timeScale = 1;
    }

    public void ReloadCheckpoint()
    {
        transform.position = CurrentCheckpoint;
        Time.timeScale = 1;
    }

    void ScaleFactor(float num = 2)
    {
        scaleFactor = num;
        transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);
    }
}