using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class High_Scores : MonoBehaviour
{
    int score1;
    string name1;
    int score2;
    string name2;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void SortScore(int score, string name)
    {
        int newScore;
        string newName;
        int oldScore;
        string oldName;
        newScore = score;
        newName = name;

        for (int i = 0; i < 10; ++i)
        {
            if (PlayerPrefs.HasKey(i + "HScore")) 
                if (PlayerPrefs.GetInt(i + "HScore") < newScore)
                {
                    oldScore = PlayerPrefs.GetInt(i + "HScore");
                    oldName = PlayerPrefs.GetString(i + "HScoreName");
                    PlayerPrefs.SetInt(i + "HScore", newScore);
                    PlayerPrefs.SetString(i + "HScoreName", newName);
                    newScore = oldScore;
                    newName = oldName;
                }
                else
                {
                    PlayerPrefs.SetInt(i + "HScore", newScore);
                    PlayerPrefs.SetString(i + "HScoreName", newName);
                    newScore = 0;
                    newName = "";
                }
        }
    }
}
