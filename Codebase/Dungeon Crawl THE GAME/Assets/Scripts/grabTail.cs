using UnityEngine;
using System.Collections;

public class grabTail : MonoBehaviour
{
    public GameObject mario;
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Tail")
           mario.GetComponent<BasePlayer>().canGrabTail = true;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Tail")
            mario.GetComponent<BasePlayer>().canGrabTail = false;
    }

}
