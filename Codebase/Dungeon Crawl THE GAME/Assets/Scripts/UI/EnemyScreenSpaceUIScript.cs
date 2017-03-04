using UnityEngine;
using UnityEngine.UI;

public class EnemyScreenSpaceUIScript : MonoBehaviour
{
    private IEnemyBehavior enemyScript;

    [SerializeField]
    private Canvas canvas;

    [SerializeField]
    public float healthPanelOffset = 0.35f;

    [SerializeField]
    private GameObject healthPanel;

    private Text enemyName;

    [SerializeField]
    private Slider healthSlider;

    private DepthUIScript depthUIScript;
    private float maxHP;

    // Use this for initialization
    private void Start()
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
    private void Update()
    {
        if (healthPanel != null)
        {
            healthSlider.value = (enemyScript.RemainingHealth() / maxHP);

            Vector3 worldPos = new Vector3(transform.position.x, transform.position.y + healthPanelOffset, transform.position.z);
            Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
            healthPanel.transform.position = new Vector3(screenPos.x, screenPos.y, screenPos.z);

            float distance = (worldPos - Camera.main.transform.position).magnitude;
            depthUIScript.depth = -distance;
            if (enemyScript.RemainingHealth() <= 0)
            {
                Destroy(healthPanel);
            }
        }
    }
}