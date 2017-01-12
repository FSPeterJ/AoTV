using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        time = time += Time.deltaTime;
    }

    void OnGUI()
    {
       GUI.Label(new Rect(x, y, 200f, 200f), time.ToString());
    }
}
