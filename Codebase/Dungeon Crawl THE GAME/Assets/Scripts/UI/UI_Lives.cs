using UnityEngine;
using UnityEngine.UI;

public class UI_Lives : MonoBehaviour
{
    private Text lives;
    private int livesvalue;

    private void OnEnable()
    {
        EventSystem.onLivesCount += UpdateLives;
    }

    //unsubscribe
    private void OnDisable()
    {
        EventSystem.onLivesCount -= UpdateLives;
    }

    // Use this for initialization
    private void Start()
    {
        lives = GetComponent<Text>();
    }

    public void UpdateLives(int _lives)
    {
        livesvalue = _lives;
    }

    // Update is called once per frame
    private void Update()
    {
        lives.text = "Lives: " + livesvalue;
    }
}