﻿using UnityEngine;
using UnityEngine.UI;

public class CanvasDialogueSystemTesting : MonoBehaviour
{
    public int maxDialogueChoices;
    public Text enemyDialogue;
    public Button positiveResponse;
    public Button negativeResponse;
    public string[] enemyDialogueArray;
    private bool dialogueSelected = false;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (dialogueSelected == false)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                enemyDialogue.text = enemyDialogueArray[0];
            }
            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                enemyDialogue.text = enemyDialogueArray[1];
            }
        }
    }

    public void PostiveResponseOneSelected()
    {
        enemyDialogue.text = enemyDialogueArray[0];
    }

    public void NegativeResponseOneSelected()
    {
        enemyDialogue.text = enemyDialogueArray[1];
    }
}