using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyActivation : MonoBehaviour {
    //public Texture key;
    bool hasKey;
    bool hasKeySoundPlayed = false;
    public GameObject interactPanel;
    public GameObject keyImage;
    void Start()
    {
        hasKey = false;
    }

    public bool HasKey
    {
        get
        {
            return hasKey;
        }

        set
        {
            hasKey = value;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (other.tag == "Player")
        //{
        //    HasKey = true;
        //    Debug.Log("has key");
        //    if (hasKeySoundPlayed != true)
        //    {
        //        GetComponent<AudioSource>().Play();
        //        hasKeySoundPlayed = true;
        //    }
        //}
        if (HasKey != true)
        {
            interactPanel.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                HasKey = true;
                Debug.Log("has key");
                keyImage.SetActive(true);
                if (hasKeySoundPlayed != true)
                {
                    GetComponent<AudioSource>().Play();
                    hasKeySoundPlayed = true;
                }
                interactPanel.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
       interactPanel.SetActive(false);
    }

    //void OnGUI()
    //{
    //    if (HasKey)
    //    {
    //        GUI.Label(new Rect(Screen.width - 116, Screen.height - 116, 115, 115), key);
    //    }
    //}
}
