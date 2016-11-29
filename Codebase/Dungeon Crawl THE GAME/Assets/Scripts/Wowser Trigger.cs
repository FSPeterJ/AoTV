using UnityEngine;
using System.Collections;

public class WowserTrigger : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("test");
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("test");
    }
    void OntriggerEnter(Collider other)
    {
        Debug.Log("test");
        Debug.Log(other);
        if(other.tag == "Player")
        {
            Debug.Log("test2");
            transform.parent.GetComponent<Wowser>().CurrentState = BossStates.Stomp;
            transform.parent.GetComponent<GameObject>().transform.Translate(0,10,0);
        }
    }
}