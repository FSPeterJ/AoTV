using UnityEngine;

public class HealthPowerUpScript : MonoBehaviour
{
    private Animator anim;
    public GameObject HealthPowerUp;
    public GameObject Gold;
    public GameObject GoldOne;
    public GameObject GoldTwo;
    public GameObject GoldThree;
    public GameObject GoldFour;
    public GameObject GoldFive;
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
                    Vector3 spawnLocation = (transform.forward * 4) + transform.position;
                    //Destroy(ChestContent);
                    spawnLocation.y += 1;
                    Destroy(Gold);
                    Destroy(GoldOne);
                    Destroy(GoldTwo);
                    Destroy(GoldThree);
                    Destroy(GoldFour);
                    Destroy(GoldFive);
                    Instantiate(HealthPowerUp, spawnLocation, Quaternion.identity);
                }
            }
        }
    }
}