using UnityEngine;
using System.Collections;

public class grabTail : MonoBehaviour
{
    Wowser WowserScript;
    private void Start()
    {
        WowserScript = GetComponent<Wowser>().GetComponent<Wowser>();
    }
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Tail")
           WowserScript.canGrabTail = true;
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Tail")
            WowserScript.canGrabTail = false;

    }

}
