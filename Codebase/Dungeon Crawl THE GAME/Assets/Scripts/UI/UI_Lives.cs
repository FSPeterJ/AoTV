using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lives : MonoBehaviour {

    Text lives;
    uint livesvalue;


    void OnEnable()
    {
        EventSystem.onLivesCount += UpdateLives;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onLivesCount -= UpdateLives;

    }

    // Use this for initialization
    void Start()
    {
        lives = GetComponent<Text>();
    }



    public void UpdateLives(uint _lives)
    {
        livesvalue = _lives;
    }

    // Update is called once per frame
    void Update()
    {
        lives.text = "Lives: " + livesvalue;
    }
}
