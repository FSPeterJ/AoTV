using UnityEngine;
using System.Collections;



public class StompCollisionTrigger : MonoBehaviour
{
    public GameObject mario;
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
           // mario.GetComponent<BasePlayer>().TakeDamage();
        }
    }
}
