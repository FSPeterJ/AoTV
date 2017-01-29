using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyActivation : MonoBehaviour {

    bool hasKey;

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
        }
    }
}
