﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject interactPanel;
    public GameObject keyCode;
    KeyActivation key;
    //public GameObject[] Lights = new GameObject[7];
    //public GameObject[] Bars = new GameObject[9];
    bool unlocked = false;
    public Transform startMarker;
    public Transform endMarker;
    public float speed = 0.000050F;
    private float startTime;
    private float journeyLength;
    public GameObject gate;
    public GameObject keyImage;
    bool unlockSoundPlayed = false;
    [SerializeField]
    bool HasKey = false;
    // Use this for initialization
    void Start()
    {
        key = keyCode.GetComponent<KeyActivation>();
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
    }


    private void OnEnable()
    {
        EventSystem.onUI_KeyCount += KeyChange;
    }

    private void OnDisable()
    {
        EventSystem.onUI_KeyCount -= KeyChange;
    }


    void KeyChange(int keys)
    {
        if (keys > 0)
        {
            HasKey = true;
        }
        else
        {
            HasKey = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (unlocked == true)
        {
            float distCovered = (Time.deltaTime - startTime) * speed;
            float fracJourney = distCovered / journeyLength;
            gate.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fracJourney);

        }
    }
    void OnTriggerEnter(Collider c)
    {
        if (HasKey == true)
        {
            if (unlocked != true)
            {
                EventSystem.UI_Interact(true);
            }
        }
    }

    void OnTriggerStay(Collider c)
    {
        
        if (c.gameObject.tag == "Player" && HasKey == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("test");
                unlocked = true;
                if (unlockSoundPlayed != true)
                {
                    GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
                    GetComponent<AudioSource>().Play();
                    unlockSoundPlayed = true;
                }
                EventSystem.UI_Interact(false);
                EventSystem.UI_KeyChange(-1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EventSystem.UI_Interact(false);
    }
}