using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider2D))]
public class MovementController : MonoBehaviour
{
    public Animator animator;
    private const float SkinWidth = .02f;
    private const int TotalHorizontalRays = 6;
    private const int TotalVerticalRays = 4;

    [Tooltip("Firing and reloading")]
    public Text AmmoUI;
    public GameObject BulletLeft, BulletRight;
    Vector2 bulletPos;
    public float fireRate = 0.5f;
    float nextFire = 0.0f;
    private int curAmmo;
    public int maxAmmo = 5;
    private bool IsReloading = false;
    public ShakeCam shaker;

    [Header("Collision Masks")]
    [Tooltip("Layers to collide with vertically.")]
    public LayerMask VerticalMask;
    [Tooltip("Layers to collide with horizontally.")]
    public LayerMask HorizontalMask;
    [Header("Parameters")]
    public ControllerParameters DefaultParameters;

    public ControllerParameters Parameters { get { return DefaultParameters; } }
    public ControllerState State { get; private set; }
    public Vector2 Velocity { get { return _velocity; } }
    public bool HandleCollisions { get; set; }
    // Is it grounded ?
    public bool IsGrounded;
    public LayerMask Ground;
    private bool IsPlaying = false;

    private Vector2 _velocity;
    private Transform _transform;
    private Vector3 _localScale;
    private BoxCollider2D _playerCollider;
    private Vector3
        _raycastTopLeft,
        _raycastBottomLeft,
        _raycastBottomRight;
    private float
        _verticalDistanceBetweenRays,
        _horizontalDistanceBetweenRays;

    public void Start()
    {
        curAmmo = maxAmmo;
    }

    public void Awake()
    {
        State = new ControllerState();
        _playerCollider = GetComponent<BoxCollider2D>();
        _transform = transform;
        _localScale = transform.localScale;
        HandleCollisions = true;

        _horizontalDistanceBetweenRays = (_playerCollider.size.x * Mathf.Abs(_localScale.x) - 2 * SkinWidth) / (TotalVerticalRays - 1);
        _verticalDistanceBetweenRays = (_playerCollider.size.y * Mathf.Abs(_localScale.y) - 2 * SkinWidth) / (TotalHorizontalRays - 1);
    }

    void Update()
    {
        if (!IsReloading)
        {
            AmmoUI.text = "Ammo : " + curAmmo.ToString();
        }
        else
        {
            AmmoUI.text = "Reloading...";
        }
    }

    public void LateUpdate()
    {
		if (!Parameters.Flying)
			ApplyGravity();
		
        Move(Velocity * Time.deltaTime);
    }
    
    void ApplyGravity()
    {
		_velocity.y += Parameters.Gravity * Time.deltaTime;
    }

    public void AddForce(Vector2 force)
    {
        _velocity += force * Time.deltaTime;
    }
    
    public void SetVelocity(Vector2 force)
    {
        _velocity = force;
    }

    public void AddHorizontalForce(float x)
    {
        _velocity.x += x * Time.deltaTime;
    }

    public void SetHorizontalVelocity(float x)
    {
        _velocity.x = x;
    }

    public void AddVerticalForce(float y)
    {
        _velocity.y += y * Time.deltaTime;
    }

    public void SetVerticalVelocity(float y)
    {
        _velocity.y = y;
    }

    void Move(Vector2 deltaMovement)
    {
        State.Reset();

        if (HandleCollisions)
        {
            CalculateRayOrigins();

			if (Mathf.Abs(deltaMovement.x) > .001f)
				MoveHorizontally(ref deltaMovement);

            MoveVertically(ref deltaMovement);
        }

		_transform.Translate(deltaMovement);

        if (Time.deltaTime > 0)

            _velocity = deltaMovement / Time.deltaTime;

        _velocity.x = Mathf.Min(_velocity.x, Parameters.MaxVelocity.x);
        _velocity.y = Mathf.Min(_velocity.y, Parameters.MaxVelocity.y);
        animator.SetFloat("Speed", deltaMovement.x);
        IsGrounded = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.5f, transform.position.y + 0.5f), Ground);
        
        if (Input.GetButtonDown("Right"))
        {
            animator.SetBool("IsMoving", true);
            animator.SetBool("IsFlipped", false);
        }

         if (Input.GetButtonDown("Left"))
        {
            animator.SetBool("IsMoving", true);
            animator.SetBool("IsFlipped", true);
        }

        if  (!Input.GetButtonDown("Right") && !Input.GetButtonDown("Left"))
        {
            if (Input.GetButtonUp("Right") || Input.GetButtonUp("Left"))
            {
                animator.SetBool("IsMoving", false);
            }
        }

        if (IsGrounded)
        {
           animator.SetBool("IsJumping", false);
           IsPlaying = false;
        }
        else
        {
           animator.SetBool("IsJumping", true);
        }
        
        //Only trigger when it is unpaused
        if (Pause.paused == false)
        {
            if (Input.GetButtonDown("Jump") && !IsPlaying)
            {
                IsPlaying = true;
                SoundsManager.PlaySound("jump");
            }
        }

        if (Input.GetButtonDown("Fire1") && Time.time > nextFire && curAmmo > 0 && !IsReloading)
        {
            animator.SetTrigger("IsShooting");
            curAmmo -= 1;
            SoundsManager.PlaySound("gun");
            nextFire = Time.time + fireRate;
            fire();
            shaker.ShakeCamera();
        }


        if (Input.GetButtonDown("Reload") || curAmmo == 0)
        {
            if (!IsReloading && curAmmo != 5)
            {
                IsReloading = true;
                StartCoroutine(Reload());
            }
        }
    }

    void CalculateRayOrigins()
    {
        var size = new Vector2(_playerCollider.size.x * Mathf.Abs(_localScale.x), _playerCollider.size.y * Mathf.Abs(_localScale.y)) / 2;
        var center = new Vector2(_playerCollider.offset.x * _localScale.x, _playerCollider.offset.y * _localScale.y);

        _raycastTopLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y + size.y - SkinWidth);
        _raycastBottomLeft = _transform.position + new Vector3(center.x - size.x + SkinWidth, center.y - size.y + SkinWidth);
        _raycastBottomRight = _transform.position + new Vector3(center.x + size.x - SkinWidth, center.y - size.y + SkinWidth);
    }

    void MoveHorizontally(ref Vector2 deltaMovement)
    {
        var isGoingRight = deltaMovement.x > 0;
        var rayDistance = Mathf.Abs(deltaMovement.x) + SkinWidth;
        var rayDirection = isGoingRight ? Vector2.right : Vector2.left;
        var rayOrigin = isGoingRight ? _raycastBottomRight : _raycastBottomLeft;

        for (var i = 0; i < TotalHorizontalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x, rayOrigin.y + i * _verticalDistanceBetweenRays);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, HorizontalMask);
			if (!raycastHit)
				continue;

			deltaMovement.x = raycastHit.point.x - rayVector.x;
            rayDistance = Mathf.Abs(deltaMovement.x);

            if (isGoingRight)
            {
                deltaMovement.x -= SkinWidth;
                State.IsCollidingRight = true;
            }
            else
            {
                deltaMovement.x += SkinWidth;
                State.IsCollidingLeft = true;
            }

            if (rayDistance < SkinWidth + .0001f)
				break;
		}

    }

    void MoveVertically(ref Vector2 deltaMovement)
    {
        var isGoingDown = deltaMovement.y < 0;
        var rayDistance = Mathf.Abs(deltaMovement.y) + SkinWidth;
        var rayDirection = isGoingDown ? Vector2.down : Vector2.up;
        var rayOrigin = isGoingDown ? _raycastBottomLeft : _raycastTopLeft;

        for (var i = 0; i < TotalVerticalRays; i++)
        {
            var rayVector = new Vector2(rayOrigin.x + i * _horizontalDistanceBetweenRays, rayOrigin.y);
            Debug.DrawRay(rayVector, rayDirection * rayDistance, Color.red);

            var raycastHit = Physics2D.Raycast(rayVector, rayDirection, rayDistance, VerticalMask);
			if (!raycastHit)
				continue;

            var hitObject = raycastHit.transform.gameObject;

			if (hitObject.CompareTag("Platform") && (!isGoingDown || State.DropThroughPlatform))
				continue;

			deltaMovement.y = raycastHit.point.y - rayVector.y;
            rayDistance = Mathf.Abs(deltaMovement.y);

            if (isGoingDown)
            {
                deltaMovement.y += SkinWidth;
                State.IsCollidingBelow = true;
            }
            else
            {
                deltaMovement.y -= SkinWidth;
                State.IsCollidingAbove = true;
            }

			if (rayDistance < SkinWidth + .0001f)
				break;
		}
    }

    void fire()
    {
        bulletPos = transform.position;
        if (!animator.GetBool("IsFlipped"))
        {
            bulletPos += new Vector2(0, -0.1f);
            Instantiate(BulletRight, bulletPos, Quaternion.identity);
        }
        else
        {
            bulletPos += new Vector2(0, -0.1f);
            Instantiate(BulletLeft, bulletPos, Quaternion.identity);
        }

    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(1.5f);
        curAmmo = maxAmmo;
        IsReloading = false;
    }
}