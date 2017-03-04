using UnityEngine;

public class HUD : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        //Score.text = "I am not updating";
    }

    public void UpdateHealth(int health)
    {
        //healthslider.value = health;
    }

    public void UpdateScore()
    {
        //++scoreCounter;
        //Score.text = "Score: " + scoreCounter;
    }

    public void PrintScore()
    {
        //Score.text = "Score: " + scoreCounter;
    }
}