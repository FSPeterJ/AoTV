using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScreenSpaceUIScript : MonoBehaviour {

    IEnemyBehavior enemyScript;
    public Canvas canvas;
    public GameObject healthPrefab;
    public float healthPanelOffset = 0.35f;
    public GameObject healthPanel;
    Text enemyName;
    Slider healthSlider;
    DepthUIScript depthUIScript;
    float maxHP;


    // Use this for initialization
    void Start ()
    {
        enemyScript = GetComponent<IEnemyBehavior>();
        healthPanel = Instantiate(healthPrefab) as GameObject;
        healthPanel.transform.SetParent(canvas.transform, false);

        enemyName = healthPanel.GetComponentInChildren<Text>();
        enemyName.text = enemyScript.Name();

        healthSlider = healthPanel.GetComponentInChildren<Slider>();

        depthUIScript = healthPanel.GetComponent<DepthUIScript>();
        canvas.GetComponent<ScreenSpaceCanvasScript>().AddToCanvas(healthPanel);
        maxHP = enemyScript.RemainingHealth();
    }
	
	// Update is called once per frame
	void Update ()
    {
        healthSlider.value = (enemyScript.RemainingHealth() / maxHP);
        Debug.Log((enemyScript.RemainingHealth() / maxHP));

        Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

        float distance = (worldPos - Camera.main.transform.position).magnitude;
        depthUIScript.depth = -distance;
    }
}
