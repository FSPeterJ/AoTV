using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //States

    enum States
    {

    }


    //Basic Settings
    public int maxJump = 1;
    public int maxJumpStored;

    //References
    CharacterController controller;

    //Physics Settings
    public float speed = 3.0F;
    public float sprintSpeed = 6.0f;
    public float jumpSpeed = 10.0F;
    public float gravity = 9.8F;
    public float mass = 20.0F;
    public float rotationSpeed = 2.0f;

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
        controller = GetComponent<CharacterController>();
        maxJumpStored = maxJump;
    }

    // Update is called once per frame
    void Update()
    {

        //Re-used a lot of Harrison's movement code
        moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= sprintSpeed;
        moveDirection *= speed;

        if (controller.isGrounded)
        {
            verticalVel = 0; // grounded character has vSpeed = 0...
            maxJump = maxJumpStored;
        }
        if (Input.GetKeyDown(KeyCode.Space) && maxJump > 0)
        {
            maxJump--;
            verticalVel -= jumpSpeed;
        }

        verticalVel += gravity * Time.deltaTime;
        moveDirection.y -= verticalVel;


        //GetComponent<Rigidbody>().transform.eulerAngles = ;
        transform.Rotate(
            0,
          0,
               0);

        //Debug.Log((float)-((Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg) - transform.rotation.y));
        Debug.Log((float)transform.rotation.y);
        Debug.Log((float)(Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg)-90 );

        controller.Move(moveDirection * Time.deltaTime);
        //Tell subscribers the player has moved
        EventSystem.PlayerPositionUpdate(transform.position);
    }

    void UpdateMousePosition(Vector3 MousePos)
    {
        mousePosition = MousePos;
    }


}



