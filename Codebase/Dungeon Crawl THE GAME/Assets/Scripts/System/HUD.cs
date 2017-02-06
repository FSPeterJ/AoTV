using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text Score;
    public int scoreCounter = 0;
    public bool dead = false;

 


    // Use this for initialization
    void Start()
    {
        Score.text = "I am not updating";
	}




    public void UpdateScore()
    {
        ++scoreCounter;
        Score.text = "Score: " + scoreCounter;
    }

    public void PrintScore()
    {
        Score.text = "Score: " + scoreCounter;
    }
}
