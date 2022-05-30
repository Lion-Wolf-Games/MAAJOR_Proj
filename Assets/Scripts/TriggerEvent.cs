using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    [SerializeField] private bool desactivateAfterTrig;
    [SerializeField] private UnityEvent events;


    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Player")
        {
            events?.Invoke();
            if(desactivateAfterTrig)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
