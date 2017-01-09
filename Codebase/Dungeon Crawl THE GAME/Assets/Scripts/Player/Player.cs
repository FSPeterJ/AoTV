using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {



    //Basic Settings
    public int maxJump = 1;

    //References
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



    // Use this for initialization
    void Start () {
        controller = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        //Re-used a lot of Harrison's movement code
        moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= sprintSpeed;
        moveDirection *= speed;

        if (Input.GetKeyDown("space") && maxJump > 0)
        {
            maxJump--;
            moveDirection.y = jumpSpeed;
        }
        moveDirection.y -= gravity * Time.deltaTime;
        transform.Rotate(0, Input.GetAxis("Horizontal") * rotationSpeed, 0);
        controller.Move(moveDirection * Time.deltaTime);
        //Tell subscribers the player has moved
        EventDelegates.PlayerPositionUpdate(transform.position);
    }


}
