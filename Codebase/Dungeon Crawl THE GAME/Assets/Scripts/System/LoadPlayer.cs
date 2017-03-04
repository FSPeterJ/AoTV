using UnityEngine;

public class LoadPlayer : MonoBehaviour
{
    public GameObject Player;

    [SerializeField]
    private float scaleFactor = 2;

    // Use this for initialization
    private void Start()
    {
        GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
        Instantiate(Player, transform.position, transform.rotation);
        EventSystem.PlayerScale(scaleFactor);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}