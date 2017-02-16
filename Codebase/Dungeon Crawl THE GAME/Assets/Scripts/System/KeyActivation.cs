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
        if (HasKey != true && other.tag == "Player")
        {
            EventSystem.UI_Interact(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && !hasKey )
            {
                hasKey = true;
                EventSystem.UI_KeyChange(1);
                Debug.Log("has key");
                if (hasKeySoundPlayed != true)
                {
                    GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
                    GetComponent<AudioSource>().Play();
                    hasKeySoundPlayed = true;
                }
                EventSystem.UI_Interact(false);
                Transform.Destroy(gameObject, 1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EventSystem.UI_Interact(false);
    }

    //void OnGUI()
    //{
    //    if (HasKey)
    //    {
    //        GUI.Label(new Rect(Screen.width - 116, Screen.height - 116, 115, 115), key);
    //    }
    //}
}
