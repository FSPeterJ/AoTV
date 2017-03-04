﻿using UnityEngine;

public class KeySystem : MonoBehaviour
{
    [SerializeField]
    private int keycount = 0;

    // Use this for initialization
    private void Start()
    {
        KeyChange(0);
    }

    private void OnEnable()
    {
        EventSystem.onUI_KeyChange += KeyChange;
    }

    private void OnDisable()
    {
        EventSystem.onUI_KeyChange -= KeyChange;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void KeyChange(int count)
    {
        keycount += count;
        EventSystem.UI_KeyCount(keycount);

        for (int i = 0; i < 4; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < keycount);
        }
    }
}