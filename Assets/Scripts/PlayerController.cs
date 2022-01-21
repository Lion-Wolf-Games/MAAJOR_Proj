using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    private Vector2 _look;
    [SerializeField] private Transform followTransform;
    [SerializeField] private Transform turnPoint;

    public Vector3 _moveInput;
    public Vector3 _move;
    public float turnSpeed;

    public float speed;
    public float maxSpeed;

    [Header("Camera")]
    public float rotationPower;
    public float aimValue;

    public Animator animator;

    [SerializeField] CharacterController controller;

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 conVec = context.ReadValue<Vector2>();

        if (_moveInput == Vector3.zero)
        {
            DOTween.To(() => 0, x => speed = x, maxSpeed, 0.25f);
            animator.SetBool("isMoving", true);
        }
        if (conVec == Vector2.zero)
        {
            DOTween.To(() => maxSpeed, x => speed = x, 0, 0.25f);
            animator.SetBool("isMoving", false);
        }

        _moveInput = conVec;
        _moveInput.z = _moveInput.y;
        _moveInput.y = 0;
    }

    public void OnMouseMove(InputAction.CallbackContext context)
    {
        _look = context.ReadValue<Vector2>();
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

        float camFacing = Camera.main.transform.eulerAngles.y;
        _move = Quaternion.Euler(0, camFacing, 0) * _moveInput;

        followTransform.transform.position = transform.position + Vector3.up;
        turnPoint.transform.position = transform.position;

        transform.rotation = Quaternion.Lerp(transform.rotation, turnPoint.rotation, turnSpeed * Time.deltaTime);

    }

    private void LateUpdate()
    {
        controller.Move(speed * Time.deltaTime * transform.forward);
        turnPoint.LookAt(new Vector3(_move.x, 0, _move.z) + turnPoint.position);

        if (!controller.isGrounded)
        {
            controller.Move(Vector3.down * 9.81f * Time.deltaTime);
        }
    }


}
