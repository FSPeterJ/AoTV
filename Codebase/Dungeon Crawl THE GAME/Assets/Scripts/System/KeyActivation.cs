using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyActivation : MonoBehaviour {
    public Texture key;
    bool hasKey;
    bool hasKeySoundPlayed = false;

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
        if (other.tag == "Player")
        {
            HasKey = true;
            Debug.Log("has key");
            if (hasKeySoundPlayed != true)
            {
                GetComponent<AudioSource>().Play();
                hasKeySoundPlayed = true;
            }
        }
    }

    void OnGUI()
    {
        if (HasKey)
        {
            GUI.Label(new Rect(Screen.width - 116, Screen.height - 116, 115, 115), key);

        }
    }
}
