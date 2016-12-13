using UnityEngine;
using System.Collections;

public class playerFollow : MonoBehaviour
{
    public GameObject[] StartCameraWaypoints;
    public GameObject player;
    bool StartingView = true;
    Vector3 offset;

    

    float rspeed = 0.4F;
    float lerpTime = 0f;
    Vector3 startpoint;
    int target = 0;

    // Use this for initialization
    void Start()
    {
        startpoint = transform.position;
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos;
        if (StartingView)
        {

            lerpTime += rspeed * Time.deltaTime;
            newPos = Vector3.Lerp(startpoint, StartCameraWaypoints[target].transform.position, lerpTime);
            if (Vector3.Distance(StartCameraWaypoints[target].transform.position, transform.position) < 0.1f)
            {
                if (target < StartCameraWaypoints.Length)
                {
                    target++;
                    startpoint = transform.position;
                    lerpTime = 0f;
                }
                else
                {
                    lerpTime = 0f;
                    StartingView = false;
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
