using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadnSave : MonoBehaviour
{
    //defining the Stream variables
    StreamReader sr;
    StreamWriter sw;

    //defining stream path variables
    string pScore;
    string currLevel;

	// Use this for initialization
	void Start()
    {
        sr = new StreamReader(pScore);
        sr = new StreamReader(currLevel);

        
	}
	
	// Update is called once per frame
	void Update()
    {
		
	}
}
