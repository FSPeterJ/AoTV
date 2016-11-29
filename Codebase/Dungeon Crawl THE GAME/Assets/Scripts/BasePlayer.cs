using UnityEngine;
using System.Collections;




public class BasePlayer : MonoBehaviour {

    // Use this for initialization
    void Start () {
	
	}


    public float Speed = 6.0F;
    public float JumpSpeed = 8.0F;
    public float Gravity = 20.0F;
    public float RotationSpeed = 2;
    private Vector3 moveDirection = Vector3.zero;
    public int numberOfJumps = 3;
    private int currentJump = 3;

    // Update is called once per frame
    void Update () {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller.isGrounded)
        {


            moveDirection = new Vector3(0, 0, Input.GetAxis("Vertical"));
            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection *= Speed;

            currentJump = numberOfJumps;            


        }



        if (Input.GetKeyDown("space") && currentJump > 0)
        {
            currentJump--;
            moveDirection.y = JumpSpeed;
            

        }
        transform.Rotate(0, Input.GetAxis("Horizontal") * RotationSpeed, 0);

        moveDirection.y -= Gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}
