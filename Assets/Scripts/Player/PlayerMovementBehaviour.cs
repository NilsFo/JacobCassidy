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

    [SerializeField]
    PlayerStateBehaviourScript _playerStateBehaviourScript;

    [SerializeField] private float speed = 10;
    
    [SerializeField] private float dashMod = 5;
    [SerializeField] private float dashDuration = 2;
    public MovementAnimator movementAnimator;

    public Transform aimDummy;

    private Vector2 _velocity;
    private Vector2 _lookDirection = new Vector2(1,0);
    
    private float _dashTime;

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
        
        var moveInput = input.Player.Move.ReadValue<Vector2>();

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
        }
    }

    private void FixedUpdate()
    {
        myRgidbody2D.MovePosition(myRgidbody2D.position + _velocity * Time.fixedDeltaTime);
    }
    
}