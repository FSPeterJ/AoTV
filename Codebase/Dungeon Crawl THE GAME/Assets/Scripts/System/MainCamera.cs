using System.Linq;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    private Camera cam;
    private Transform Rail;
    private RaycastHit hitInfo;
    private int layerMask = 31;
    private float scaleFactor = 2;

    [SerializeField]
    private float minZoom = -10;

    [SerializeField]
    private float maxZoom = 0;

    [SerializeField]
    private float zoomOffset = -10;

    [SerializeField]
    private float sensitivity = 10f;

    //subscribe to player movement
    private void OnEnable()
    {
        EventSystem.onPlayerPositionUpdate += UpdateTargetPosition;
        EventSystem.onPlayerScale += ScaleFactor;
    }

    //unsubscribe from player movement
    private void OnDisable()
    {
        EventSystem.onPlayerPositionUpdate -= UpdateTargetPosition;
        EventSystem.onPlayerScale -= ScaleFactor;
    }

    //public Transform target;
    private Vector3 targetpos;

    public float smoothTime = 0.3f;

    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        Rail = transform.GetChild(0);
        cam = Rail.GetChild(0).GetComponent<Camera>();
    }

    private void FixedUpdate()
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
        Rail.transform.localPosition = new Vector3(0, 0.5f * scaleFactor, 0);
        Rail.localPosition = new Vector3(0, 2, Mathf.Clamp(zoom, minZoom * scaleFactor + zoomOffset * scaleFactor, maxZoom * scaleFactor + zoomOffset * scaleFactor));
    }

    private void UpdateTargetPosition(Vector3 pos)
    {
        targetpos = pos;
    }

    private void PlayerDied()
    {
    }

    private void ScaleFactor(float num)
    {
        scaleFactor = num;
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
}