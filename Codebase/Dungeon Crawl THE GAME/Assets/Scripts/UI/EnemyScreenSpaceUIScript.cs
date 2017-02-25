using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScreenSpaceUIScript : MonoBehaviour {

    IEnemyBehavior enemyScript;
    [SerializeField]
    Canvas canvas;
    [SerializeField]
    public float healthPanelOffset = 0.35f;
    [SerializeField]
    GameObject healthPanel;
    Text enemyName;
    [SerializeField]
    Slider healthSlider;
    DepthUIScript depthUIScript;
    float maxHP;


    // Use this for initialization
    void Start ()
    {
        healthPanel = Instantiate((GameObject)Resources.Load("Prefabs/UI/UI_EnemyHealth"));
        enemyScript = GetComponent<IEnemyBehavior>();
        canvas = GameObject.FindGameObjectWithTag("UI Main").GetComponent<Canvas>();
        healthPanel.transform.SetParent(canvas.transform, false);


        enemyName = healthPanel.GetComponentInChildren<Text>();
        enemyName.text = enemyScript.Name();

        healthSlider = healthPanel.GetComponentInChildren<Slider>();

        depthUIScript = healthPanel.GetComponent<DepthUIScript>();
        canvas.GetComponent<ScreenSpaceCanvasScript>().AddToCanvas(healthPanel);
        maxHP = enemyScript.RemainingHealth();
        healthPanelOffset = enemyScript.HPOffsetHeight();
        healthSlider.value = (enemyScript.RemainingHealth() / maxHP);

    }

    // Update is called once per frame
    void Update ()
    {
        healthSlider.value = (enemyScript.RemainingHealth() / maxHP);

        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

        float distance = (worldPos - Camera.main.transform.position).magnitude;
        depthUIScript.depth = -distance;
    }
}
