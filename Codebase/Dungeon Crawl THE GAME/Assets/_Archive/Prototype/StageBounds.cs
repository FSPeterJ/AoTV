using UnityEngine;
using System.Collections;

public class StageBounds : MonoBehaviour {


    bool Activated = false;
    public Wowser wowserScript;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Wowser")
        {
            if(wowserScript.CurrentState != BossStates.Spawning)
            wowserScript.CurrentState = BossStates.Falling;
        }
    }
    void OnTriggerEnter(Collider collision)
    {
        if(!Activated && collision.transform.tag == "Player")
        {
            Activated = true;
            wowserScript.CurrentState = BossStates.Spawning;
        }
    }
}
