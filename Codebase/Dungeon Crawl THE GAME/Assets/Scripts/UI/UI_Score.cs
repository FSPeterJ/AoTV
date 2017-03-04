using UnityEngine;
using UnityEngine.UI;

public class UI_Score : MonoBehaviour
{
    private Text score;
    private int scorevalue;

    private void OnEnable()
    {
        EventSystem.onScoreChange += UpdateScore;
    }

    //unsubscribe
    private void OnDisable()
    {
        EventSystem.onScoreChange -= UpdateScore;
    }

    // Use this for initialization
    private void Start()
    {
        score = GetComponent<Text>();
    }

    public void UpdateScore(int _score)
    {
        scorevalue = _score;
    }

    // Update is called once per frame
    private void Update()
    {
        score.text = "Score: " + scorevalue;
    }
}