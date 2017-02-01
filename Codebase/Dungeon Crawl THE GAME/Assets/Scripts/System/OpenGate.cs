using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject interactPanel;
    public GameObject keyCode;
    KeyActivation key;
    public GameObject[] Lights = new GameObject[7];
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
    // Use this for initialization
    void Start()
    {
        key = keyCode.GetComponent<KeyActivation>();
        startTime = Time.time;
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
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
        if (key.HasKey == true)
        {
            if (unlocked != true)
            {
                interactPanel.SetActive(true);
            }
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (key.HasKey == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                unlocked = true;
                if (unlockSoundPlayed != true)
                {
                    GetComponent<AudioSource>().Play();
                    unlockSoundPlayed = true;
                }
                interactPanel.SetActive(false);
                keyImage.SetActive(false);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactPanel.SetActive(false);        
    }
}