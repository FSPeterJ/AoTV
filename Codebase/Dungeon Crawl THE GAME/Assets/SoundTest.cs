using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTest : MonoBehaviour {
    public AudioClip sfx;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TestSFX()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        GetComponent<AudioSource>().PlayOneShot(sfx);
    }

    public void TestMusic()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");
        GetComponent<AudioSource>().Play();
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}
