using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Tutorial : MonoBehaviour {

    public Text[] Info;
    public Image[] arrows;

    bool firstItem = true;
    bool secondItem = false;
    bool thirdItem = false;


    // Use this for initialization
    void Start ()
    {
        Info[0].enabled = true;
        arrows[0].enabled = true;
	}
	
	// Update is called once per frame
	void Update () {

        if (firstItem)
        {
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftShift))
            {
                firstItem = false;
                Info[0].enabled = false;
                arrows[0].enabled = false;
                secondItem = true;
                Info[1].enabled = true;
                arrows[1].enabled = true;
            }
        }

        if (secondItem)
        {
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                secondItem = false;
                Info[1].enabled = false;
                arrows[1].enabled = false;
                thirdItem = true;
                Info[2].enabled = true;
                arrows[2].enabled = true;
            }
        }

        if (thirdItem)
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                thirdItem = false;
                Info[2].enabled = false;
                arrows[2].enabled = false;
            }
        }
    }
}
