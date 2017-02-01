using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_Testing : MonoBehaviour {
    public Texture key;
	// Use this for initialization
	void Start ()
    {
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width - 116, Screen.height -116, 115, 115), key);
    }
}
