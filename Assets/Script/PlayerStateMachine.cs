using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class PlayerStateMachine : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;
    public float gravity = -15f;

    [Header("Visuals")]
    public Color idleColor = Color.black;
    public Color walkColor = new Color(0.2f, 0.6f, 1f);
    public Color jumpColor = new Color(0.2f, 1f, 0.2f);
    public Color hideColor = Color.grey;

    // Properti publik
    public CharacterController Controller { get; private set; }
    public Renderer CharacterRenderer { get; private set; }
    public TextMeshPro StatusText { get; private set; }
    public Vector3 Velocity { get; set; }
    public float HorizontalInput { get; private set; }
    public bool IsHidden { get; set; } = false;

    // State saat ini
    private PlayerBaseState _currentState;

    // Instance untuk setiap state
    public PlayerIdleState IdleState { get; } = new PlayerIdleState();
    public PlayerWalkingState WalkingState { get; } = new PlayerWalkingState();
    public PlayerInAirState InAirState { get; } = new PlayerInAirState();
    public PlayerHidingState HidingState { get; } = new PlayerHidingState();

    void Awake()
    {
        Controller = GetComponent<CharacterController>();
        CharacterRenderer = GetComponent<Renderer>();
        StatusText = GetComponentInChildren<TextMeshPro>();
    }

    void Start()
    {
        ChangeState(IdleState);
    }

    void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");
        _currentState?.UpdateState(this);
    }
    
    public void ChangeState(PlayerBaseState newState)
    {
        _currentState?.ExitState(this);
        _currentState = newState;
        _currentState.EnterState(this);
    }

    // --- FUNGSI KUNCI UNTUK MELOMPAT ---
    public void Jump()
    {
        if (!Controller.isGrounded) return;

        // Terapkan fisika lompatan
        Velocity = new Vector3(Velocity.x, Mathf.Sqrt(jumpHeight * -2f * gravity), Velocity.z);
        
        // Setelah fisika diterapkan, BARU ubah state
        ChangeState(InAirState);
    }
}