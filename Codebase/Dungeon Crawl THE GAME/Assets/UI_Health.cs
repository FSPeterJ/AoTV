using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour {

    Slider healthslider;


    void OnEnable()
    {
        EventSystem.onPlayerHealthUpdate += UpdateHealth;
    }
    //unsubscribe
    void OnDisable()
    {
        EventSystem.onPlayerHealthUpdate -= UpdateHealth;

    }

    // Use this for initialization
    void Start()
    {
        healthslider = GetComponent<Slider>();
    }



    public void UpdateHealth(int health, int healthmax)
    {
        healthslider.value = health;
        healthslider.maxValue = healthmax;
    }
}
