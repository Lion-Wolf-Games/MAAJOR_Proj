using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    [SerializeField] private float timeActive;

    private float disableTime;
    private void Start() {
        disableTime = Time.time+ timeActive;
    }

    private void Update() {
        if (Time.time >= disableTime)
        {
            gameObject.SetActive(false);
        }
    }
}
