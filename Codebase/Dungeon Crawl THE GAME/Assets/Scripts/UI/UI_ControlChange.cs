using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.Events;

public class UI_ControlChange : MonoBehaviour
{

    static GameObject CurrentControl;

    // Use this for initialization
    void Start()
    {

        CurrentControl = null;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform tf = transform.GetChild(0);

            KeyCode keycode;
            string temp = PlayerPrefs.GetString(tf.name);
            if (temp.Length > 0)
            {
                keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), temp);
                if (keycode > 0)
                    KeyManager.SetKey(tf.name, keycode);
            }
            tf.GetChild(0).GetComponent<Text>().text = KeyManager.GetKeyName(tf.name);
            tf = transform.GetChild(1);
            temp = PlayerPrefs.GetString(tf.name);
            if (temp.Length > 0)
            {
                keycode = (KeyCode)System.Enum.Parse(typeof(KeyCode), temp);
                if (keycode > 0)
                    KeyManager.SetKey(tf.name, keycode);
            }

            tf.GetChild(0).GetComponent<Text>().text = KeyManager.GetKeyName(tf.name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnGUI()
    {
        if (CurrentControl != null)
        {
            Event e = Event.current;
            if (e != null && (e.isKey))
            {
                KeyManager.SetKey(CurrentControl.name, e.keyCode);
                PlayerPrefs.SetString(CurrentControl.name, e.keyCode.ToString());
                CurrentControl.GetComponent<Image>().color = Color.white;

                CurrentControl.transform.GetChild(0).GetComponent<Text>().text = KeyManager.GetKeyName(CurrentControl.name);
                CurrentControl = null;
                PlayerPrefs.Save();
            }

            else if (e != null && e.isMouse)
            {

                KeyManager.SetKey("Mousebutton " + CurrentControl.name, e.keyCode);
                PlayerPrefs.SetString("Mousebutton " + CurrentControl.name, e.keyCode.ToString());
                CurrentControl.GetComponent<Image>().color = Color.white;

                CurrentControl.transform.GetChild(0).GetComponent<Text>().text = KeyManager.GetKeyName("Mousebutton " + CurrentControl.name);
                CurrentControl = null;
                PlayerPrefs.Save();

            }
        }
    }


    public void AssignControl(GameObject inputName)
    {
        if (CurrentControl)
        {
            CurrentControl.GetComponent<Image>().color = Color.white;
        }
        CurrentControl = inputName;
        CurrentControl.GetComponent<Image>().color = Color.red;
    }
}
