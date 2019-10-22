﻿using UnityEngine;

public class Player : MonoBehaviour
{
    [Tooltip("Jump strenght.")]
    public float JumpMagnitude = 12f;

    [Tooltip("The amount of force used to interrupt a jump.")]
    public float JumpInterruptStrength = 120f;

    [Tooltip("How long can the player still be considered grounded after leaving the ground?")]
	public float GroundedLinger = 0.05f;
    
    [Tooltip("The distance below the player where jump input is registered while falling.")]
	public float GroundCheckDistance = 0.5f;

    [Tooltip("Will the player be able to slide off walls?")]
    public bool WallSlide = true;

    [Tooltip("How fast does the player slide off walls?")]
    [Range(0, 1)]
    public float WallFriction = 0.5f;

    [Tooltip("Is wall jumping allowed?")]
    public bool WallJump = true;

    [Tooltip("Direction and strength of wall jump")]
    public Vector2 WallJumpForce = new Vector2(12, 12);

    [Tooltip("How long can wall jump still be performed after not touching a wall?")]
    public float WallLinger = 0.1f;

    public bool Jumpping { get; set; }
    public bool JumpWhenGrounded { get; set; }
    public bool IsGrounded
    {
        get
        {
            if (_controller.State.IsCollidingBelow)
            {
                _groundLingerTime = 0;
                return true;
            }
            if (_groundLingerTime < GroundedLinger)
                return true;

            return false;
        }
    }
	public bool GroundIsNear
    {
    	get
        {
            var rayOrigin = new Vector2(_transform.position.x, _transform.position.y + _playerCollider.offset.y - _playerCollider.size.y / 2 - 0.01f);
            var rayHit = Physics2D.Raycast(rayOrigin, Vector2.down, GroundCheckDistance);
            Debug.DrawRay(rayOrigin, Vector2.down * GroundCheckDistance, Color.green);
            return rayHit;
        }
    }
	public bool AnticipateJump { get { return !IsGrounded && GroundIsNear && _controller.Velocity.y < 0; } }
    public bool IsTouchingWall { get { return _controller.State.IsCollidingLeft || _controller.State.IsCollidingRight; } }
    public bool CanWallJump { get { return WallJump && (IsTouchingWall || _wallLingerTime < WallLinger); } }

    private enum Walls {left, rigth};

	private bool _isFacingRight;
	private float _normalizedHorizontalSpeed;
	private float _groundLingerTime;
    private float _wallLingerTime;
    private Walls _lastWallTouched;

	private Transform _transform;
	private BoxCollider2D _playerCollider;
	private MovementController _controller;

	void Awake()
    {
		_transform = transform;
		_playerCollider = GetComponent<BoxCollider2D>();
		_controller = GetComponent<MovementController>();
		_isFacingRight = _transform.localScale.x > 0;
    }

	void Update()
	{
        _groundLingerTime += Time.deltaTime;
        if (IsTouchingWall)
        {
            if (_controller.State.IsCollidingLeft)
                _lastWallTouched = Walls.left;
            else _lastWallTouched = Walls.rigth;
            _wallLingerTime = 0;
        }
        else _wallLingerTime += Time.deltaTime;

        if (_controller.Velocity.y < 0)
            Jumpping = false;

        if (WallSlide && IsTouchingWall && _controller.Velocity.y <= 0)
        {
            if (WallFriction == 1)
                _controller.Parameters.Flying = true;
            _controller.SetVerticalVelocity(_controller.Velocity.y * (1 - WallFriction));
        }
        else _controller.Parameters.Flying = false;
        
		HandleInput();
		
		var acceleration = IsGrounded ? _controller.Parameters.AccelerationOnGround : _controller.Parameters.AccelerationInAir;
		
		_controller.SetHorizontalVelocity(Mathf.Lerp(_controller.Velocity.x, _normalizedHorizontalSpeed * _controller.Parameters.MaxSpeed, Time.deltaTime * acceleration));
    }

	void HandleInput()
	{
		_normalizedHorizontalSpeed = Input.GetAxis("Horizontal");

		if ((Input.GetButton("Left") && _isFacingRight && !Input.GetButton("Right")) ||
 		    (Input.GetButton("Right") && !_isFacingRight && !Input.GetButton("Left")))
			Flip();

        if (AnticipateJump && Input.GetButtonDown("Jump"))
            JumpWhenGrounded = true;

		if ((Input.GetButtonDown("Jump") && IsGrounded && !Jumpping) || (JumpWhenGrounded && IsGrounded)) 
			Jump(JumpMagnitude);

        else if (CanWallJump && Input.GetButtonDown("Jump"))
            JumpOffWall(WallJumpForce);

		if (Jumpping && !Input.GetButton("Jump"))
			_controller.AddVerticalForce(-JumpInterruptStrength);

		_controller.State.DropThroughPlatform = Input.GetButton("Down");
	}

    void Jump(float magnitude)
    {
        JumpWhenGrounded = false;
        Jumpping = true;
        _controller.SetVerticalVelocity(magnitude);
    }

    void JumpOffWall(Vector2 force)
    {
        JumpWhenGrounded = false;
        Jumpping = true;
        var jumpVector = new Vector2(_lastWallTouched == Walls.left ? force.x : -force.x, force.y);
        _controller.SetVelocity(jumpVector);
    }

	void Flip()
	{
	    _transform.localScale = new Vector3(-_transform.localScale.x, _transform.localScale.y, _transform.localScale.z);
		_isFacingRight = !_isFacingRight;
	}
}