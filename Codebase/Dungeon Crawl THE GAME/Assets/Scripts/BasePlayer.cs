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

    // Use this for initialization
    void Start()
    {

    }

    public int HP = 3;
    public float speed = 6.0F;
    public float sprintSpeed = 10.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float rotationSpeed = 2.0f;
    private Vector3 moveDirection = Vector3.zero;
    public Rigidbody marioRb;
    public Rigidbody wowserRb;
    public playerStates currentState = playerStates.normal;
    public int numberOfJumps = 3;
    private int currentJump = 3;
    public bool canGrabTail = false;
    private bool tailGrabbed = false;
    public float tossSpeed;
    public float tossSeconds = 1.5f;
    public GameObject getTail;
    public GameObject Wowser;
    public GameObject Life1;
    public GameObject Life2;
    public GameObject Life3;


    private bool hasThrown = false;



    // Update is called once per frame
    void Update()
    {
        if (canGrabTail ==true)
        {
            Debug.Log("CanGrabTail = true");
        }
        CharacterController controller = GetComponent<CharacterController>();

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
                Wowser.transform.parent = null;
                tailGrabbed = false;

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D) && !hasThrown)
                {
                    StartCoroutine("tossTime");

                    //Need to find wowser's front vector and move him in that direction
                }
                currentState = playerStates.normal;
            }
            else
            {
                Wowser.GetComponent<NavMeshAgent>().enabled = false;

                currentState = playerStates.throwing;
                Wowser.transform.parent = transform;
                tailGrabbed = true;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        controller.Move(moveDirection * Time.deltaTime);

        if (transform.position.y < -10)
        {
            transform.position = new Vector3(0, 10 ,0);
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        switch(HP)
        {
            case 3:
                Life3.GetComponent<Renderer>().enabled = false;
                break;
            case 2:
                Life2.GetComponent<Renderer>().enabled = false;

                break;
            case 1:
                Life1.GetComponent<Renderer>().enabled = false;
                SceneManager.LoadScene("KillMario");
                break;
        }
        HP -= 1;

    }
    void TakeFireDamage(int damage)
    {

        TakeDamage();
    }

    IEnumerator tossTime()
    {
        hasThrown = true;
        Wowser.GetComponent<Wowser>().CurrentState = BossStates.Idle;
        Wowser.GetComponent<Rigidbody>().isKinematic = false;
        Wowser.GetComponent<Rigidbody>().velocity = (transform.forward * tossSpeed);
        yield return new WaitForSeconds(tossSeconds);
        Wowser.GetComponent<Rigidbody>().isKinematic = true;

        //Wowser.GetComponent<Wowser>().CurrentState = BossStates.Moving;

        hasThrown = false;

    }
}