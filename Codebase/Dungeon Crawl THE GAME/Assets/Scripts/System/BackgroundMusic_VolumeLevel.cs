using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic_VolumeLevel : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
