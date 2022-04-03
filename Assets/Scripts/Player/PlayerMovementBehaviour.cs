using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementBehaviour : MonoBehaviour
{
    [Header("Dependency")] [SerializeField]
    private Rigidbody2D myRgidbody2D;
    public GameStateBehaviourScript gameState;
    MainInputActionsSettings input;
    private PlayerInput _playerInput;

    [SerializeField] PlayerStateBehaviourScript _playerStateBehaviourScript;

    [SerializeField] private float speed = 10;

    [SerializeField] private float dashMod = 5;
    [SerializeField] private float dashDuration = 2;
    public MovementAnimator movementAnimator;

    public Transform aimDummy;

    Vector2 _velocity;

    Vector2 _lookDirection = new Vector2(1,0);
    public Vector2 lookDirection => _lookDirection;
    public Vector2 velocity => _velocity;

    private float _dashTime;
    
    public float movementCooldown = 0;

    private void Awake()
    {
        Debug.Assert(myRgidbody2D != null, gameObject);
    }

    private void Start() {
        input = gameState.mainInputActions;
        _playerInput = FindObjectOfType<PlayerInput>();
        _playerInput.actions = input.asset;
    }

    private void Update()
    {
        if (input.Player.Dash.triggered && _playerStateBehaviourScript.ChangeCurrentStamina(-1))
        {
            _dashTime = dashDuration;
        }

        if (_dashTime > Time.deltaTime)
        {
            _dashTime -= Time.deltaTime;
        }
        else
        {
            _dashTime = 0;
        }


        var modSpeed = speed;
        if (_dashTime > 0)
        {
            modSpeed *= dashMod;
        }

        bool movementBlocked = false;
        // Check if movement is on cooldown
        if (movementCooldown > 0) {
            movementBlocked = true;
            movementCooldown -= Time.deltaTime;
        }
        
        var moveInput = input.Player.Move.ReadValue<Vector2>();
        if (movementBlocked) {
            moveInput = Vector2.zero;
        }

        _velocity = moveInput;
        _velocity = _velocity.normalized;
        
        _velocity *= modSpeed;
        
        movementAnimator.velocity = _velocity;
        
        // Look direction
        Vector2 lookInput = Vector2.zero;

        if (_playerInput.currentControlScheme.Equals("Gamepad")) {
            lookInput = input.Player.Look.ReadValue<Vector2>();
        } else if (_playerInput.currentControlScheme.Equals("Keyboard&Mouse")) {
            var mousePos = Mouse.current.position.ReadValue();
            var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            lookInput = worldPos -transform.position;
        }
        

        
        if (lookInput.magnitude > 0.1f) {
            _lookDirection = lookInput.normalized;
            aimDummy.localPosition = _lookDirection;
            movementAnimator.SetFacing(_lookDirection);
        }
    }

    private void FixedUpdate()
    {
        myRgidbody2D.AddForce(_velocity * Time.fixedDeltaTime * 1000);
    }

    public void Knockback(Vector2 sourcePosition, float force)
    {
        Vector2 angle = (Vector2)transform.position - sourcePosition;
        angle = angle.normalized;
        myRgidbody2D.AddForce(angle * force);
    }
    public void TakeDamage() {
        var sprite = movementAnimator.GetComponent<SpriteRenderer>();
        if (sprite != null) {
            sprite.color = new Color(.5f,.25f,.25f);
            Invoke(nameof(TakeDamageEnd), 0.2f);    
        }
        
    }

    public void TakeDamageEnd() {
        
        var sprite = movementAnimator.GetComponent<SpriteRenderer>();

        if (sprite != null) {
            sprite.color = Color.white;
        }
    }
}
