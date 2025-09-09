using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class EnemyStateMachine : MonoBehaviour
{
    [Header("AI Properties")]
    public Transform player;
    public Transform[] waypoints;
    public float moveSpeed = 3f;
    public float chaseSpeed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -15f;

    [Header("AI Senses")]
    public float detectionRadius = 10f;
    public float attackRange = 2f;

    [Header("Visuals")]
    public Color patrolColor = Color.cyan;
    public Color chaseColor = Color.yellow;
    public Color attackColor = Color.red;
    public Color jumpColor = new Color(0.2f, 1f, 0.2f); // Warna BARU untuk lompat

    // Properti publik
    public CharacterController Controller { get; private set; }
    public Renderer CharacterRenderer { get; private set; }
    public TextMeshPro StatusText { get; private set; }
    public Vector3 Velocity { get; set; }
    public int CurrentWaypointIndex { get; set; } = 0;
    public bool IsFacingRight { get; set; } = true;

    // State saat ini
    private EnemyBaseState _currentState;

    // Instance untuk setiap state
    public EnemyPatrolState PatrolState { get; } = new EnemyPatrolState();
    public EnemyChaseState ChaseState { get; } = new EnemyChaseState();
    public EnemyAttackState AttackState { get; } = new EnemyAttackState();
    public EnemyInAirState InAirState { get; } = new EnemyInAirState();

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        CharacterRenderer = GetComponent<Renderer>();
        StatusText = GetComponentInChildren<TextMeshPro>();
    }

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned in the inspector!");
            return;
        }
        ChangeState(PatrolState);
    }

    void Update()
    {
        if (player == null) return;
        _currentState?.UpdateState(this);
    }
    
    public void ChangeState(EnemyBaseState newState)
    {
        _currentState?.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    public void Jump()
    {
        if (!Controller.isGrounded) return;

        Velocity = new Vector3(Velocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity), Velocity.z);
        
        ChangeState(InAirState);
    }

    public void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;

        if (StatusText != null)
        {
            Vector3 textScale = StatusText.transform.localScale;
            textScale.x *= -1;
            StatusText.transform.localScale = textScale;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}