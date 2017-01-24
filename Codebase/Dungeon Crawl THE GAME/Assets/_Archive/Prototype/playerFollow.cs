using UnityEngine;
using System.Collections;

public class playerFollow : MonoBehaviour
{
    public GameObject[] StartCameraWaypoints;
    public GameObject player;
    bool StartingView = true;
    Vector3 offset;

    

    float rspeed = 0.9F;
    float lerpTime = 0f;
    Vector3 startpoint;
    int target = 0;

    // Use this for initialization
    void Start()
    {
        startpoint = transform.position;
        offset = new Vector3(0, 20f, -13f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F2))
        {
            transform.position = new Vector3(0, 5, 0);
            StartingView = false;
            player.GetComponent<BasePlayer>().DisableControls = false;
        }

        Vector3 newPos;
        if (StartingView)
        {

            lerpTime += rspeed * Time.deltaTime;
            newPos = Vector3.Lerp(startpoint, StartCameraWaypoints[target].transform.position, lerpTime);
            if (Vector3.Distance(StartCameraWaypoints[target].transform.position, transform.position) < 0.1f)
            {
                if (target < StartCameraWaypoints.Length - 1)
                {
                    target++;
                    startpoint = transform.position;
                    lerpTime = 0f;
                }
                else
                {
                    lerpTime = 0f;
                    StartingView = false;
                    player.GetComponent<BasePlayer>().DisableControls = false;
                }
                    
            }
        }
        else
        {

            newPos = player.transform.position + offset;
        }

        transform.position = newPos;
        
    }

   

}
