using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour {


    Text score;
    int scorevalue;


    void OnEnable()
    {
        EventSystem.onScoreChange += UpdateScore;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onScoreChange -= UpdateScore;

    }

    // Use this for initialization
    void Start()
    {
        score = GetComponent<Text>();
    }



    public void UpdateScore(int _score)
    {
        scorevalue = _score;
    }

	
	// Update is called once per frame
	void Update () {
        score.text = "Score: " + scorevalue;
	}

}
