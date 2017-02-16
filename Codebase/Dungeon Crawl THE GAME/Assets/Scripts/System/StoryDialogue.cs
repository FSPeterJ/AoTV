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

    [SerializeField]
    AudioClip[] EnemyDialogueResponses;

    float timer;

    bool secondLevel;

    bool rumble;

    bool starting = false;

    void EnableDia()
    {
        if (!starting)
        {
            transform.GetChild(0).gameObject.SetActive(true);
            Mage.GetComponent<AudioSource>().PlayOneShot(EnemyDialogueResponses[0]);
            timer = .03f;
            starting = true;
            secondLevel = false;
            rumble = false;
        }
    }

    // Use this for initialization
    void Start()
    {
        EventSystem.onStoryDialogue += EnableDia;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (starting)
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
    }

    void DialogueBranch_0_1()
    {
        Mage.GetComponent<AudioSource>().Stop();
        Mage.GetComponent<AudioSource>().PlayOneShot(EnemyDialogueResponses[1]);
        EnemyDialogue.text = "You will not gain access to this Crypt.";
        PlayerResponse1.text = "(3) Death Knight defies the natural order. He will be reaped.";
        PlayerResponse2.text = "(4) I don't have time for this. Give me the key or be reaped.";
        secondLevel = true;
    }

    void DialogueBranch_0_2()
    {
        Mage.GetComponent<AudioSource>().Stop();
        Mage.GetComponent<AudioSource>().PlayOneShot(EnemyDialogueResponses[2]);
        EnemyDialogue.text = "So be it then.";
        PlayerResponse1.text = "";
        PlayerResponse2.text = "";
        rumble = true;
        //Start Combat
    }
}