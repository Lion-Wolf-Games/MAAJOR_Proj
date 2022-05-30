using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;
using Cinemachine;

public class PlayerController : LivingObject
{
    [Space]
    [SerializeField] CharacterController controller;
    private Vector2 _look;
    [SerializeField] private Transform followTransform;
    [SerializeField] private Transform turnPoint;

    public Vector3 _moveInput;
    public float joystickDeadzone;
    public bool _jumpInput;
    public bool _isGrounded;
    public LayerMask walkableLayers;
    public Vector3 _move;
    public float turnSpeed;
    public float _jumpForce;
    private float _verticalSpeed;
    public float speed;
    public float maxSpeed;
    private float baseSpeed;

    [Header("Throw")]
    public bool isAiming;
    [SerializeField] private bool hasThrow ;
    private bool throwInput;
    private float trhowValue; 

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

    [Header("Suck")]
    public Transform suckPosition;
    [SerializeField] private float suckSpeedFactor = 0.5f;
    [SerializeField] private float suckRange;
    [SerializeField] private LayerMask enemyLayer;
    
 
    [Header("Camera")]
    public CinemachineVirtualCamera tPSCam;
    public float rotationPower;
    public float aimValue;

    #region Actions
    public delegate void PlayerActions();

    public PlayerActions OnDash;
    public PlayerActions OnJump;
    public PlayerActions OnLanding;
    public PlayerActions OnSuck;
    public PlayerActions OnStartSuck;
    public PlayerActions OnStopSuck;
    public PlayerActions OnStartAim;
    public PlayerActions OnStopAim;
    public PlayerActions OnThrow;

    public PlayerActions NextPotion,PrevPotion;

    #endregion
    

    //public Animator animator;
    private PlayerInput input;

    private void Start() 
    {
        input = GetComponent<PlayerInput>();
        GameManager.Instance.OnPlay += OnGamePlay;
        OnKill += (x) => {StartCoroutine("RestartLevel");};
        input.SwitchCurrentActionMap("Player");

        baseSpeed = maxSpeed;
    }

    IEnumerator RestartLevel()
    {
        DisableInput();
        yield return new WaitForSeconds(2f);
        GameManager.StaticLoadLevel(1);
    }

    #region Inputs
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
        if (context.started)
        {
            if (Time.timeScale == 1)
            {
                GameManager.Instance.ChangeGameState(GameState.Paused);
                input.SwitchCurrentActionMap("UI");
            }
            else
            {
                GameManager.Instance.ChangeGameState(GameState.Playing);
                input.SwitchCurrentActionMap("Player");
            }
        }

    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
    }

    public void Dash(InputAction.CallbackContext context)
    {

        if ((context.performed) && (dashTime <= Time.time))
        {
            isDashing = true;
            dashTime = Time.time + dashDuration;
            dashDir = transform.forward;
            animator.SetTrigger("Roll");
            
            OnDash?.Invoke();
        }
    }

    public void Suck(InputAction.CallbackContext context)
    {
        // if(context.performed)
        // {
        //     Debug.Log("Suck");
        //     OnSuck?.Invoke();

        //     Collider[] cols = Physics.OverlapSphere(transform.position, suckRange/*,enemyLayer*/);

        //     Debug.Log(cols);

        //     foreach (var col in cols)
        //     {
        //         col.BroadcastMessage("OnSuck",this,SendMessageOptions.DontRequireReceiver);
        //     }

        //     StartCoroutine(DisableInputTemporary(1));
        //     animator.SetTrigger("Suck");
        // }

        //Start Suck
        if(context.performed)
        {
            //Slow pim Mouvement
            maxSpeed = baseSpeed * suckSpeedFactor;
            OnStartSuck?.Invoke();
        }

        //Stop suck
        if(context.canceled)
        {
            //Reset Pim Speed
            maxSpeed = baseSpeed;
            OnStopSuck?.Invoke();
        }
    } 

    public void Aim(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            DOTween.To(() => 0.75f, x => tPSCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraSide = x, 1f, 0.25f);

            isAiming = true;
            OnStartAim?.Invoke();
        }
        else if (context.canceled)
        {
            DOTween.To(() => 1f, x => tPSCam.GetCinemachineComponent<Cinemachine3rdPersonFollow>().CameraSide = x, 0.75f, 0.25f);
            isAiming = false;
            OnStopAim?.Invoke();
        }
    }

    public void Throw(InputAction.CallbackContext context)
    {
        //trhowValue = context.ReadValue<float>();
        //Debug.Log(trhowValue);

        if (context.started && isAiming)
        {
            Debug.Log("Start");
            OnThrow.Invoke();
            //animator.SetTrigger("Throw");
        }
        else if(context.canceled)
        {
            Debug.Log ("Stop");
        }
    }

    public void SwitchPotion(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            float value = context.ReadValue<float>();

            Debug.Log(value);

            if(value >0)
            {
                PrevPotion?.Invoke();
            }
            else if(value <0)
            {
                NextPotion?.Invoke();
            }
        }

    }

    #endregion

    private void Update()
    {
        #region Follow Transform Rotation

        // if(_look.magnitude > 1)
        // {
        //     _look.Normalize();
        // }

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

        if (Physics.OverlapSphere(transform.position, 0.25f, walkableLayers).Length > 0 && _verticalSpeed < 1)
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

        #region Jump
        if (!_isGrounded)
        {
            _verticalSpeed -= 12f * Time.deltaTime;

            _jumpInput = false;

        }
        else if (_verticalSpeed != -3)
        {
            _verticalSpeed = -3;

            OnLanding?.Invoke();
        }

        if (_jumpInput && _isGrounded)
        {
            _verticalSpeed = _jumpForce;
            controller.Move(Vector3.up * _verticalSpeed * Time.deltaTime);

            animator.SetTrigger("isJumping");

            //Debug.Log("is Jumping");

            OnJump?.Invoke();
        }

        #endregion

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

        if(trhowValue > 0.5f && !hasThrow && isAiming)
        {
            OnThrow.Invoke();
            animator.SetTrigger("Throw");
            hasThrow = true;
        }

        if(trhowValue < 0.2 && hasThrow)
        {
            hasThrow = false;
        }
    }

    private void OnGamePlay()
    {
        input.SwitchCurrentActionMap("Player");
    }

    protected override void Hit(int damage,Vector3 origin)
    {
        animator.SetInteger("DeathID", UnityEngine.Random.Range(0, 2));

        if (_health <= 0)
        {
            DisableInput();
        }

        base.Hit(damage,origin);
        OnHit?.Invoke(0);
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

    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, 0.25f);
    }

    private IEnumerator DisableInputTemporary(float time)
    {
        DisableInput();
        yield return new WaitForSeconds(time);
        EnableInput();
    }
}
