using UnityEngine;
using System.Collections;

public class grabTail : MonoBehaviour
{
    public Wowser WowserScript;
    private void Start()
    {
        WowserScript = GetComponent<Wowser>();
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
