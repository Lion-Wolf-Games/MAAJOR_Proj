using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public Action OnApply;
    public Action OnCancel;

    static public SettingsManager Instance;
    private void Awake()
    {
        Instance = this;
    }

    public void ApplySettings()
    {
        Instance.OnApply?.Invoke();
    }

    public void CancelChanges()
    {
        Instance.OnCancel?.Invoke();
    }
}
