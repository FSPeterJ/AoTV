using UnityEngine;

public class BackgroundMusic_VolumeLevel : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");
    }

    // Update is called once per frame
    private void Update()
    {
    }
}