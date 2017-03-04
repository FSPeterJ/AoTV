using UnityEngine;
using UnityEngine.UI;

public class UI_Health : MonoBehaviour
{
    private Slider healthslider;

    private void OnEnable()
    {
        EventSystem.onPlayerHealthUpdate += UpdateHealth;
    }

    //unsubscribe
    private void OnDisable()
    {
        EventSystem.onPlayerHealthUpdate -= UpdateHealth;
    }

    // Use this for initialization
    private void Start()
    {
        healthslider = GetComponent<Slider>();
    }

    public void UpdateHealth(int health, int healthmax)
    {
        healthslider.value = health;
        healthslider.maxValue = healthmax;
    }
}