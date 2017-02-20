using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginBossBattle : MonoBehaviour {
    
    public Dragon drag;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            drag.SendMessage("StartFight");
        }
    }
}
