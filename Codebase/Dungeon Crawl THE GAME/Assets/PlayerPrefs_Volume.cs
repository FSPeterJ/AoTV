using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerPrefs_Volume : MonoBehaviour {

    public Slider SFXSlider;
    public Slider MusicSlider;


	// Use this for initialization
	void Start ()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        MusicSlider.value = PlayerPrefs.GetFloat("Music Volume");
    }
	
	// Update is called once per frame
	void Update () {
        VolumeControl();
	}

    void VolumeControl()
    {
        PlayerPrefs.SetFloat("SFX Volume", SFXSlider.value);
        PlayerPrefs.SetFloat("Music Volume", MusicSlider.value);
        PlayerPrefs.Save();
    }
}
