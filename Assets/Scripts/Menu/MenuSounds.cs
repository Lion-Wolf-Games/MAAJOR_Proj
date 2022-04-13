using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuSounds : MonoBehaviour
{
    public AK.Wwise.Event OnMenuOpen;
    public AK.Wwise.Event OnMenuClose;
    public AK.Wwise.Event OnNavigate;

    private void OnEnable()
    {
        OnMenuOpen.Post(gameObject);
    }
    private void OnDisable()
    {
        OnMenuClose.Post(gameObject);
    }

    public void NavigationSFX(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnNavigate.Post(gameObject);
        }
    }
}
