using UnityEngine;

public class OpenGate : MonoBehaviour
{
    public GameObject interactPanel;
    public GameObject keyCode;
    private KeyActivation key;

    //public GameObject[] Lights = new GameObject[7];
    //public GameObject[] Bars = new GameObject[9];
    private bool unlocked = false;

    public Transform startMarker;
    public Transform endMarker;
    public float speed = 0.000050F;
    public GameObject gate;
    public GameObject keyImage;
    private bool unlockSoundPlayed = false;

    [SerializeField]
    private bool HasKey = false;

    // Use this for initialization
    private void Start()
    {
        //key = keyCode.GetComponent<KeyActivation>();
    }

    private void OnEnable()
    {
        EventSystem.onUI_KeyCount += KeyChange;
    }

    private void OnDisable()
    {
        EventSystem.onUI_KeyCount -= KeyChange;
    }

    private void KeyChange(int keys)
    {
        if (keys > 0)
        {
            HasKey = true;
        }
        else
        {
            HasKey = false;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (unlocked == true && transform.position.y > -200)
        {
            gate.transform.position -= new Vector3(0, 1 * Time.deltaTime, 0);
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (HasKey == true && c.tag == "Player")
        {
            if (unlocked != true)
            {
                EventSystem.UI_Interact(true);
            }
        }
    }

    private void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Player" && HasKey == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                unlocked = true;
                if (unlockSoundPlayed != true)
                {
                    GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("SFX Volume");
                    GetComponent<AudioSource>().Play();
                    unlockSoundPlayed = true;
                }
                EventSystem.UI_Interact(false);
                EventSystem.UI_KeyChange(-1);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EventSystem.UI_Interact(false);
    }
}