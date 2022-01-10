using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugPlayer : MonoBehaviour
{
    [SerializeField] private GameObject tPCam;

    /*public void CamSwitch(InputAction.CallbackContext context)
    {
        if (tPCam.activeInHierarchy)
        {
            tPCam.SetActive(false);
        }
        else
        {
            tPCam.SetActive(true);
        }
    }*/

    private void Update()
    {
        if (Keyboard.current.cKey.wasPressedThisFrame)
        {
            if (tPCam.activeInHierarchy)
            {
                tPCam.SetActive(false);
            }
            else
            {
                tPCam.SetActive(true);
            }
        }
    }
}
