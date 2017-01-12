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
        
        //mousePosition = GetComponent<Camera>().ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z - GetComponent<Camera>().transform.position.z));
        //GetComponent<Rigidbody>().transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2((mousePosition.y - transform.position.y), (mousePosition.x - transform.position.x)) * Mathf.Rad2Deg - 90);

        //transform.Rotate(0, 0, 0);
        
        controller.Move(moveDirection * Time.deltaTime);
        //Tell subscribers the player has moved
        EventSystem.PlayerPositionUpdate(transform.position);

    }
}



// using UnityEngine;
// using System.Collections;
 
// public class Bacteria_Controller : MonoBehaviour
//{

//    //Public Vars
//    public Camera camera;
//    public float speed;

//    //Private Vars
//    private Vector3 mousePosition;
//    private Vector3 direction;
//    private float distanceFromObject;

//    void FixedUpdate()
//    {

//        if (Input.GetButton("Fire2"))
//        {

//            //Grab the current mouse position on the screen

//            //Rotates toward the mouse

//            //Judge the distance from the object and the mouse
//            distanceFromObject = (Input.mousePosition - camera.WorldToScreenPoint(transform.position)).magnitude;

//            //Move towards the mouse
//            rigidbody.AddForce(direction * speed * distanceFromObject * Time.deltaTime);

//        }//End Move Towards If Case

//    }//End Fire3 If case
//}
     
// }