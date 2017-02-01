using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUI_DialogueSytemTest : MonoBehaviour
{
    public string originalStatement;
    public string ResponseOne;
    public string ResponseTwo;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2, Screen.height/2 + 200, 115, 115), originalStatement);
        GUI.Button(new Rect(Screen.width / 2 - 200, Screen.height / 2 + 300, 115, 115), ResponseOne);
        GUI.Button(new Rect(Screen.width / 2 + 200, Screen.height / 2 + 300, 115, 115), ResponseTwo);
    }
}
