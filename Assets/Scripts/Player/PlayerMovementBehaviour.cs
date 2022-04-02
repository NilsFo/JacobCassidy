using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementBehaviour : MonoBehaviour
{
    [Header("Dependency")] [SerializeField]
    private Rigidbody2D myRgidbody2D;

    [SerializeField] PlayerStateBehaviourScript _playerStateBehaviourScript;

    [SerializeField] private float speed = 10;

    [SerializeField] private float dashMod = 5;
    [SerializeField] private float dashDuration = 2;
    public MovementAnimator movementAnimator;

    private Vector2 _velocity;

    private float _dashTime;

    private void Awake()
    {
        Debug.Assert(myRgidbody2D != null, gameObject);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Dash") && _playerStateBehaviourScript.ChangeCurrentStamina(-1))
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

        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var verticalInput = Input.GetAxisRaw("Vertical");

        var modSpeed = speed;
        if (_dashTime > 0)
        {
            modSpeed *= dashMod;
        }

        _velocity = new Vector2(horizontalInput, verticalInput);
        _velocity = _velocity.normalized;

        _velocity *= modSpeed;

        movementAnimator.velocity = _velocity;
    }

    private void FixedUpdate()
    {
        myRgidbody2D.MovePosition(myRgidbody2D.position + _velocity * Time.fixedDeltaTime);
    }

    public void Knockback(Vector2 sourcePosition, float force)
    {
        Vector2 angle = (Vector2)transform.position - sourcePosition;
        angle = angle.normalized;
        myRgidbody2D.AddForce(angle * force);
    }
}