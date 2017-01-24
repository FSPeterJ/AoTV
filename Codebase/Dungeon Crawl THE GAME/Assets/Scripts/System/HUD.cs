using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    //Score variables
    int pScore;
    int sX = Screen.width - 1;
    int sY = 0;
    bool killEnemy = false;

    //Timer variables
    int x = Screen.width / 2;
    int y = 0;
    float Timer = 0.0f;

    //Life variables
    int pLives;
    int lX = 0;
    int lY = 0;

    // Use this for initialization
    void Start()
    {
        pScore = 0;
        pLives = 5;
	}
	
	// Update is called once per frame
	void Update()
    {
        Timer += Time.deltaTime;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(x, y, 200f, 200f), Timer.ToString());
        GUI.Label(new Rect(sX, sY, 75f, 75f), pScore.ToString());
    }
}
