using UnityEngine;
using System.Collections;




public class BasePlayer : MonoBehaviour {

    // Use this for initialization
    void Start () {
	
	}

    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    private Vector3 moveDirection = Vector3.zero;
    public int numberOfJumps = 3;
    private int currentJump = 3;

    // Update is called once per frame
    void Update () {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {

            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);
            moveDirection *= speed;

            currentJump = numberOfJumps;            

        }

        if (Input.GetKeyDown("space") && currentJump > 0)
        {
            currentJump--;
            moveDirection.y = jumpSpeed;
            
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
