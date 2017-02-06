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

    float timer = 3;

    bool secondLevel = false;

    bool rumble = false;

	// Use this for initialization
	void Start ()
    {
		
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

        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) && secondLevel == true)
        {
            DialogueBranch_0_2();
            rumble = true;
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
        PlayerResponse1.text = "(1) Don't tell me what to do old man";
        PlayerResponse2.text = "(2) At least I'm fighting half a mage";
        secondLevel = true;
    }

    void DialogueBranch_0_2()
    {
        EnemyDialogue.text = "So Uncivilized.";
        PlayerResponse1.text = "";
        PlayerResponse2.text = "";
        //Start Combat
    }
}
