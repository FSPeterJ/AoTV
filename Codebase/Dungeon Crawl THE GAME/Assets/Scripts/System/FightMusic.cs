using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightMusic : MonoBehaviour {
    public GameObject BackgroundMusic;
	// Use this for initialization
	void Start () {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");
        TurnOff();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void TurnOn()
    {
        BackgroundMusic.GetComponent<AudioSource>().mute = true;
        GetComponent<AudioSource>().Play();
    }

    public void TurnOff()
    {
        BackgroundMusic.GetComponent<AudioSource>().mute = false;
        GetComponent<AudioSource>().Stop();
    }
}
