using UnityEngine;

public class Cheat_Death : MonoBehaviour
{
    private Animator anim;
    public GameObject HealthPowerUp;
    private bool notOpen = true;

    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
    }

    private void OnTriggerStay(Collider Col)
    {
        if (Col.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (notOpen == true)
                {
                    notOpen = false;
                    anim.SetBool("Open", true);
                    Vector3 spawnLocation = transform.position;
                    spawnLocation.x += 4;
                    Instantiate(HealthPowerUp, spawnLocation, Quaternion.identity);
                }
            }
        }
    }
}