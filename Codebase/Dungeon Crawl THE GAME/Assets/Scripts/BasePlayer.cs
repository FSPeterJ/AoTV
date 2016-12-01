using UnityEngine;
using System.Collections;




public class BasePlayer : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    public int HP = 100;
    public float speed = 6.0F;
    public float sprintSpeed = 10.0f;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    public float rotationSpeed = 2.0f;
    private Vector3 moveDirection = Vector3.zero;
    public int numberOfJumps = 3;
    private int currentJump = 3;
    private bool canGrabTail = false;

    public GameObject getTail;
    public GameObject Wowser;

    // Update is called once per frame
    void Update()
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {

            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            Wowser.transform.parent = transform;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);

        controller.Move(moveDirection * Time.deltaTime);
    }
    void OnCollisionEnter (Collision col)
    {
        if (col.gameObject == getTail)
            canGrabTail = true;
    }
    void OnCollisionExit (Collision col)
    {
        if (col.gameObject == getTail)
            canGrabTail = false;
    }

    void TakeDamage(int damage)
    {
        HP -= damage;
    }

    void TakeFireDamage(int damage)
    {

        TakeDamage(damage);
    }
}
