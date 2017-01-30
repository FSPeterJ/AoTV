using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    Camera cam;
    RaycastHit hitInfo;
    int layerMask = 31;


    public float minFov = 15f;
    public float maxFov = 90f;
    public float sensitivity = 10f;

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

    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, smoothTime);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hitInfo, layerMask))
        {
            Debug.DrawLine(Camera.main.transform.position, hitInfo.point, Color.red);
            // Do something with the object that was hit by the raycast.
            EventSystem.MousePositionUpdate(hitInfo.point);
        }

        //Saved time with just using this:
        //http://answers.unity3d.com/answers/218373/view.html
        float fov = Camera.main.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        Camera.main.fieldOfView = fov;


    }

    void UpdateTargetPosition(Vector3 pos)
    {
        targetpos = pos;
        targetpos.y = targetpos.y + 14;
        targetpos.z = targetpos.z - 10;
    }

    void PlayerDied()
    {
        
    } 

}
