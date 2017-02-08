using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryDialogue : MonoBehaviour
{
    [SerializeField]
    GameObject Mage;
    [SerializeField]
    GameObject DialoguePanel;
    [SerializeField]
    Text EnemyDialogue;
    [SerializeField]
    Text PlayerResponse1;
    [SerializeField]
    Text PlayerResponse2;

    float timer;

    bool secondLevel;

    bool rumble;

	// Use this for initialization
	void Start ()
    {
        timer = 3;
        secondLevel = false;
        rumble = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1) && secondLevel == false)
        {
            DialogueBranch_0_1();
        }

        if (Input.GetKeyUp(KeyCode.Alpha2) && secondLevel == false)
        {
            DialogueBranch_0_2();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) && secondLevel == true)
        {
            DialogueBranch_0_2();
        }

        if (rumble)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            Mage.SendMessage("Fight");
            DialoguePanel.SetActive(false);
        }
    }

    void DialogueBranch_0_1()
    {
        EnemyDialogue.text = "You will not gain access to this Crypt.";
        PlayerResponse1.text = "(3) Don't tell me what to do old man";
        PlayerResponse2.text = "(4) At least I'm fighting half a mage";
        secondLevel = true;
    }

    void DialogueBranch_0_2()
    {
        EnemyDialogue.text = "So Uncivilized.";
        PlayerResponse1.text = "";
        PlayerResponse2.text = "";
        rumble = true;
        //Start Combat
    }
}
