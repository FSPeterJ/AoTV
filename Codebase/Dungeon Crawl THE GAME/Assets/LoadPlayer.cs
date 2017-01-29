using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour {
    public GameObject Player;
    public GameObject SpawnPoint;
	// Use this for initialization
	void Start ()
    {
        Instantiate(Player, SpawnPoint.transform);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
