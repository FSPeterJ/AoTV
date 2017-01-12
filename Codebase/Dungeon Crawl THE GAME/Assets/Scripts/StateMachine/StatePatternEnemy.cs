using UnityEngine;
public class StatePatternEnemy : MonoBehaviour
{
    public float searchingTurnSpeed = 120f;//Speed at which the enemy is going to turn to meet the player
    public float searchingDuration = 4f;//How long the enemy will search for the player in alert mode
    public float sightRange = 20f;//How far to raycast to see the player
    public GameObject[] wayPoints;//Points to patrol to
    public Transform eyes;//Raycast origin
    public Vector3 offset = new Vector3(0, .5f, 0);//Lift raycast to look at players head
    //public MeshRenderer meshRendererFlag;//Cube above enemies head

    [HideInInspector]
    public Transform chaseTarget;//reference players transform

    [HideInInspector]
    public IEnemyState currentState;//current state

    [HideInInspector]
    public ChaseState chaseState;//chase state

    [HideInInspector]
    public AlertState alertState;//alert state

    [HideInInspector]
    public PatrolState patrolState;//patrol state

    [HideInInspector]
    public UnityEngine.AI.NavMeshAgent navMeshAgent;//

    Animator anim;

    private void Awake()//Before Start, initializes states and gets component reference for NavMeshAgent attached to enemy
    {
        chaseState = new ChaseState(this);
        alertState = new AlertState(this);
        patrolState = new PatrolState(this);

        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    // Use this for initialization
    private void Start()
    {
        currentState = patrolState;//Initializes first state to patrolling
    }

    // Update is called once per frame
    private void Update()
    {
        currentState.UpdateState();//Each class has an updateState. This function behavior will differ depending on the current state
        Debug.Log(currentState.ToString());

    }
}