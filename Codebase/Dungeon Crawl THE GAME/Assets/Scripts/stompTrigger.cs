using UnityEngine;
using System.Collections;

public class stompTrigger : MonoBehaviour {


    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        Debug.Log("Left");
    //        //GetComponentInParent<Wowser>().CurrentState = BossStates.Moving;

    //    }
    //}
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if(GetComponentInParent<Wowser>().CurrentState == BossStates.Moving)
            {
                Debug.Log("Player Enter");
                GetComponentInParent<Wowser>().CurrentState = BossStates.Stomp;
            }
            //transform.parent.GetComponent<GameObject>().transform.Translate(0, 10, 0);
        }
    }
}
