using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text Score;
    public int scoreCounter = 0;
    public bool dead = false;

    public Slider healthslider;

    // Use this for initialization
    void Start()
    {
        Score.text = "Score: " + scoreCounter;
	}



    public void UpdateHealth(int health)
    {
        healthslider.value = health;
    }

    public void UpdateScore()
    {
        ++scoreCounter;
        Score.text = "Score: " + scoreCounter;
    }
}
