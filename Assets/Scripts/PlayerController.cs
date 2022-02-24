using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : LivingObject
{
    private Vector2 _look;
    [SerializeField] private Transform followTransform;
    [SerializeField] private Transform turnPoint;

    public Vector3 _moveInput;
    public float joystickDeadzone;
    public bool _jumpInput;
    public bool _isGrounded;
    public Vector3 _move;
    public float turnSpeed;
    public float _jumpForce;
    private float _verticalSpeed;
    public float speed;
    public float maxSpeed;

    [Header("Dash")]
    public float dashDuration;
    public AnimationCurve dashCurve;
    private float dashTime;
    private Vector3 dashDir;
    public bool isDashing;

    [Header("Hit")]
    public float hitRecoilDuration;
    private Vector3 hitDir;
    private float hitTime;
    public AnimationCurve hitForce;

    [Header("Camera")]
    public float rotationPower;
    public float aimValue;

    [Header("FX")]
    [SerializeField] private GameObject smokeParticle;
    [SerializeField] private GameObject grassParticle;
    [SerializeField] private GameObject jumpFx;
    [SerializeField] private GameObject DashFx;

    [SerializeField] private Transform smokeSpawnPoint;
    [SerializeField] private Transform grassSpawnPoint;

    //public Animator animator;
    private PlayerInput input;
    [SerializeField] CharacterController controller;

    private void Start() 
    {
        input = GetComponent<PlayerInput>();
        GameManager.Instance.OnPlay += OnGamePlay;
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 conVec = context.ReadValue<Vector2>();

        if (_moveInput == Vector3.zero)
        {
            //DOTween.To(() => 0, x => speed = x, maxSpeed, 0.25f);
            animator.SetBool("isMoving", true);
        }
        if (conVec == Vector2.zero)
        {
            DOTween.To(() => speed, x => speed = x, 0, 0.25f);
            animator.SetBool("isMoving", false);
        }

        _moveInput = conVec;
        _moveInput.z = _moveInput.y;
        _moveInput.y = 0;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            _jumpInput = context.performed;
        }
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.ChangeGameState(GameState.Paused);
            input.SwitchCurrentActionMap("UI");
        }
        
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {
        
        if( (context.performed) && (dashTime <= Time.time))
        {
            isDashing = true;
            dashTime = Time.time + dashDuration;
            dashDir = transform.forward;
            PoolManager.Instantiate(DashFx,transform.position,transform.rotation);
            animator.SetTrigger("Roll");
        }
    }

    private void Update()
    {
        #region Follow Transform Rotation

        //Rotate the Follow Target transform based on the input
        followTransform.transform.rotation *= Quaternion.AngleAxis(_look.x * rotationPower, Vector3.up);

        #endregion

        #region Vertical Rotation
        followTransform.transform.rotation *= Quaternion.AngleAxis(-_look.y * rotationPower, Vector3.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;

        var angle = followTransform.transform.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        followTransform.transform.localEulerAngles = angles;
        #endregion

        #region Ground Check

        if (Physics.OverlapSphere(transform.position, 0.25f, 1 << 6).Length > 0 && _verticalSpeed < 1)
        {
            _isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
        else
        {
            _isGrounded = false;
            animator.SetBool("isGrounded", false);
        }

        #endregion

        float camFacing = Camera.main.transform.eulerAngles.y;
        _move = Quaternion.Euler(0, camFacing, 0) * _moveInput;

        followTransform.transform.position = transform.position + Vector3.up;
        turnPoint.transform.position = transform.position;

        speed = Mathf.Lerp(speed, _moveInput.magnitude * maxSpeed, Time.deltaTime * 5);
        animator.SetFloat("MoveSpeed", _moveInput.magnitude);
        transform.rotation = Quaternion.Lerp(transform.rotation, turnPoint.rotation, turnSpeed * Time.deltaTime);


    }

    private void LateUpdate()
    {

        if (!_isGrounded)
        {
            _verticalSpeed -= 9.81f * Time.deltaTime;

            _jumpInput = false;

        }
        else if (_verticalSpeed != -3)
        {
            _verticalSpeed = -3;
        }

        if (_jumpInput && _isGrounded)
        {
            _verticalSpeed = _jumpForce;
            controller.Move(Vector3.up * _verticalSpeed * Time.deltaTime);

            animator.SetTrigger("isJumping");

            Debug.Log("is Jumping");

            if (jumpFx != null)
            {
                Instantiate(jumpFx, transform.position, Quaternion.identity);
            }
        }

        #region Dash
        if(dashTime >= Time.time && isDashing)
        {
            float t = (dashTime - Time.time) / dashDuration;
            t = Mathf.Abs(t-1);
            float dashVelocity = dashCurve.Evaluate(t);
            controller.Move(dashVelocity * Time.deltaTime * dashDir);
        }
        if(dashTime < Time.time && isDashing)
        {
            isDashing = false;
        }
        #endregion
        
        #region HitRecoil
        if(hitTime >= Time.time)
        {
            float t = (hitTime - Time.time) / hitRecoilDuration;
            t = Mathf.Abs(t - 1);
            float hitVelocity = hitForce.Evaluate(t);
            controller.Move(hitVelocity * Time.deltaTime * hitDir);
        }
        #endregion

        controller.Move(Vector3.up * _verticalSpeed * Time.deltaTime);
        controller.Move(speed * Time.deltaTime * transform.forward);
        turnPoint.LookAt(new Vector3(_move.x, 0, _move.z) + turnPoint.position);
    }

    private void OnGamePlay()
    {
        input.SwitchCurrentActionMap("Player");
    }

    protected override void Hit(int damage,Vector3 origin)
    {
        base.Hit(damage,origin);
        hitTime = Time.time + hitRecoilDuration;
        if(origin == transform.position)
        {
            hitDir = -transform.forward;
        } else
        {                
            hitDir = transform.position - origin;
        }
    }

    public void DisableInput()
    {
        input.enabled = false;
        _moveInput = Vector3.zero;
        isDashing = false;
    }

    public void EnableInput()
    {
        input.enabled = true;
    }

    public void Move(Vector3 velocity)
    {
        controller.Move(velocity);
    }

    public void Step()
    {
        Instantiate(smokeParticle, smokeSpawnPoint.position, Quaternion.identity);
        Instantiate(grassParticle, grassSpawnPoint.position, Quaternion.identity);
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }
}
