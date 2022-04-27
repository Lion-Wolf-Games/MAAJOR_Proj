using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    [SerializeField] private float timeActive;
    private bool isOn;

    private float disableTime;
    
    private void OnEnable() {
        disableTime = Time.time+ timeActive;
        isOn = true;
    }

    private void Update() {
        if (isOn && Time.time >= disableTime)
        {
            isOn = false;
            gameObject.SetActive(false);
        }
    }
}
