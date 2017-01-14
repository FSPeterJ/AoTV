using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    Camera cam;
    RaycastHit hitInfo;
    int layerMask = 31;

    //subscribe to player movement
    void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
    }


    //public Transform target;
    Vector3 targetpos;
    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;


    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, smoothTime);
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10;

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hitInfo, layerMask))
        {
            //Transform objectHit = hit.transform;
            // Do something with the object that was hit by the raycast.
            EventSystem.MousePositionUpdate(hitInfo.point);
        }


        
    }

    void UpdateTargetPosition(Vector3 pos)
    {
        targetpos = pos;
        targetpos.y = targetpos.y + 14;
        targetpos.z = targetpos.z - 10;
    }
}
