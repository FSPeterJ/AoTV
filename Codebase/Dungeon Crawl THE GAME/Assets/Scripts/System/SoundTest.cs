using UnityEngine;

public class SoundTest : MonoBehaviour
{
    public AudioClip SFX;

    public void TestSFX()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        GetComponent<AudioSource>().PlayOneShot(SFX);
    }

    public void TestMusic()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("Music Volume");
        GetComponent<AudioSource>().Play();
    }

    public void StopMusic()
    {
        GetComponent<AudioSource>().Stop();
    }
}