using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadPlayer : MonoBehaviour {
    public GameObject Player;
    [SerializeField]
    float scaleFactor = 2;

	// Use this for initialization
	void Start ()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        Instantiate(Player,transform.position,transform.rotation);
        EventSystem.PlayerScale(scaleFactor);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
