using UnityEngine;

public class Timer : MonoBehaviour
{
    //defining the variables needed
    private float time = -10.5f;

    private int x = Screen.width / 2;
    private int y = 0;

    // Use this for initialization
    private void Start()
    {
        Update();
    }

    // Update is called once per frame
    private void Update()
    {
        //increasing time by adding deltatime
        time += Time.deltaTime;
    }

    private void OnGUI()
    {
        //displaying the timer
        GUI.Label(new Rect(x, y, 200f, 200f), time.ToString());
    }
}