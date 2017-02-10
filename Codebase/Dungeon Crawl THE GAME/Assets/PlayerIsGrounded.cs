using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIsGrounded : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    int collisionCount = 0;
    bool Notified = false;


    private void OnTriggerEnter(Collider other)
    {
        if (collisionCount == 0)
        {
            Notified = false;
        }
        collisionCount++;
        Debug.Log(collisionCount);
    }
    private void OnTriggerExit(Collider other)
    {
        collisionCount--;
        if (collisionCount == 0)
        {
            Notified = false;
        }
        Debug.Log(collisionCount);
    }

    void Update()

    {
        if(Notified == false)
        {
            if ( collisionCount == 0 )
            {
                EventSystem.PlayerGrounded(false);
                Notified = true;
            }
            else
            {
                EventSystem.PlayerGrounded(true);
                Notified = true;
            }
        }
        
         
    }

}
