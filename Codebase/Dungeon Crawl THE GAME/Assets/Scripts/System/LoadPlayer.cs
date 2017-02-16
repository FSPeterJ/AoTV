using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour {
    public GameObject Player;
    public GameObject SpawnPoint;
    Transform spawn;
	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        spawn = SpawnPoint.transform;
        Instantiate(Player, spawn);
        Player.transform.localScale = new Vector3(.5f, .5f, .5f);
        SpawnPoint.transform.DetachChildren();
        Player.transform.position = SpawnPoint.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
