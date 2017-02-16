using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MainCamera : MonoBehaviour
{

    Camera cam;
    Transform Rail;
    RaycastHit hitInfo;
    int layerMask = 31;
    float scaleFactor = 2;

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
        EventSystem.onPlayerScale += ScaleFactor;
    }
    //unsubscribe from player movement
    void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        EventSystem.onPlayerScale -= ScaleFactor;
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
        RaycastHit[] temp = Physics.RaycastAll(ray).OrderBy(h => h.distance).ToArray();

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
        Rail.localPosition = new Vector3(0, 2, Mathf.Clamp(zoom, minZoom * scaleFactor + zoomOffset * scaleFactor,  maxZoom * scaleFactor + zoomOffset * scaleFactor));


    }

    void UpdateTargetPosition(Vector3 pos)
    {
        targetpos = pos;

    }

    void PlayerDied()
    {

    }

    void ScaleFactor(float num)
    {
        scaleFactor = num;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }

}
