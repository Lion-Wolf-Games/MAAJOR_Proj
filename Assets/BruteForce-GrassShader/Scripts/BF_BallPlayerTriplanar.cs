using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class BF_BallPlayerTriplanar : MonoBehaviour
{
    public Camera cam;
    public Transform planet;
    private Rigidbody rb;
    private Quaternion camRot;
    private Vector3 moveDirection;
    private Vector3 inputDirection;

    [HideInInspector]
    public Vector3 gravityCustom = Vector3.zero;
    private Vector3 newCameraPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        if(cam == null)
        {
            cam = Camera.main;
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        inputDirection = Vector3.zero;
        inputDirection = Vector3.zero;
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current.qKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            inputDirection += new Vector3(0, 0, 1);
        }
        if (Keyboard.current.dKey.isPressed)
        {
            inputDirection += new Vector3(0, 0, -1);
        }
        if (Keyboard.current.wKey.isPressed || Keyboard.current.zKey.isPressed)
        {
            inputDirection += new Vector3(1, 0, 0);
        }
        if (Keyboard.current.sKey.isPressed)
        {
            inputDirection += new Vector3(-1, 0, 0);
        }
#else
        if (Input.GetKey(KeyCode.Q)|| Input.GetKey(KeyCode.A))
        {
            inputDirection += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputDirection += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W))
        {
            inputDirection += new Vector3(1, 0, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputDirection += new Vector3(-1, 0, 0);
        }
#endif
        CustomGravity();
        MoveBall();
        MoveCamera();
    }

    private void MoveBall()
    {
        camRot = Quaternion.AngleAxis(cam.transform.rotation.eulerAngles.y, Vector3.up);
        moveDirection = camRot * new Vector3(Mathf.Clamp(inputDirection.x * 2, -1, 1), Mathf.Clamp(inputDirection.z * 2, -1, 1), 0f);
        rb.AddTorque(moveDirection*9.5f);
    }

    private void MoveCamera()
    {
        newCameraPos = gravityCustom * -7.2f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, newCameraPos, Time.deltaTime * 10f);
        Quaternion targetRotation = Quaternion.LookRotation(gravityCustom);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, targetRotation, Time.deltaTime * 10f);
    }

    private void CustomGravity()
    {
        gravityCustom = (planet.position - this.transform.position).normalized;
        rb.AddForce(gravityCustom * 5f,ForceMode.Acceleration);
    }
}
