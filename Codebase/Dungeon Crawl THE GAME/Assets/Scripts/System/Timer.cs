using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    //defining the variables needed
    float time = -10.5f;
    int x = Screen.width / 2;
    int y = 0;

	// Use this for initialization
	void Start()
    {
        Update();
	}
	
	// Update is called once per frame
	void Update()
    {
        //increasing time by adding deltatime
        time = time += Time.deltaTime;
    }

    void OnGUI()
    {
        //displaying the timer
       GUI.Label(new Rect(x, y, 200f, 200f), time.ToString());
    }
}
