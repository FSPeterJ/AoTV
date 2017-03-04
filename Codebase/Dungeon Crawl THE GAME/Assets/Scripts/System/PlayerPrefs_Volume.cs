using UnityEngine;
using UnityEngine.UI;

public class PlayerPrefs_Volume : MonoBehaviour
{
    public Slider SFXSlider;
    public Slider MusicSlider;

    // Use this for initialization
    private void Start()
    {
        SFXSlider.value = PlayerPrefs.GetFloat("SFX Volume");
        MusicSlider.value = PlayerPrefs.GetFloat("Music Volume");
    }

    // Update is called once per frame
    private void Update()
    {
        VolumeControl();
    }

    private void VolumeControl()
    {
        PlayerPrefs.SetFloat("SFX Volume", SFXSlider.value);
        PlayerPrefs.SetFloat("Music Volume", MusicSlider.value);
        PlayerPrefs.Save();
    }
}