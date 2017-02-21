using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour {


    Text score;
    int scorevalue;


    void OnEnable()
    {
        EventSystem.onScoreIncrease += UpdateScore;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onScoreIncrease -= UpdateScore;

    }

    // Use this for initialization
    void Start()
    {
        score = GetComponent<Text>();
    }



    public void UpdateScore(int _score)
    {
        scorevalue += _score;
    }

	
	// Update is called once per frame
	void Update () {
        score.text = "Score: " + scorevalue;
	}

}
