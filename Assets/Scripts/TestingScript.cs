using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestingScript : MonoBehaviour
{
    public PlayerInput input;
    private string currentControlScheme;
 
    private void Update() {
        if (input.currentControlScheme != currentControlScheme)
        {
            currentControlScheme = input.currentControlScheme;
            Changedcontrol(input);
        }
    }


    public void Changedcontrol(PlayerInput pi)
    {
        Debug.Log("inputChanged");
    }
}
