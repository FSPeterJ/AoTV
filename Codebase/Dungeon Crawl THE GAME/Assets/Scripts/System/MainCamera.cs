using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    Camera cam;
    Transform Rail;
    RaycastHit hitInfo;
    int layerMask = 31;

    [SerializeField]
    float minZoom = -10;
    [SerializeField]
    float maxZoom = 0;
    [SerializeField]
    float zoomOffset = -10;
    [SerializeField]
    float sensitivity = 10f;

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
        Rail = transform.GetChild(0);
        cam = Rail.GetChild(0).GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, targetpos, ref velocity, smoothTime);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] temp = Physics.RaycastAll(ray);

        for (int i = 0; i < temp.Length; i++)
        {
            if (temp[i].collider.gameObject.layer == layerMask)
            {
                Debug.DrawLine(Camera.main.transform.position, temp[i].point, Color.red);

                // Do something with the object that was hit by the raycast.
                EventSystem.MousePositionUpdate(temp[i].point);
                break;
            }

        }


        float zoom = Rail.localPosition.z;
        zoom -= Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        Rail.localPosition = new Vector3(0, 2, Mathf.Clamp(zoom, minZoom + zoomOffset,  maxZoom+ zoomOffset));


    }

    void UpdateTargetPosition(Vector3 pos)
    {
        targetpos = pos;

    }

    void PlayerDied()
    {

    }

}
