using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SuckableObject : MonoBehaviour
{
    [SerializeField] private float suckTime;
    [SerializeField] private float suckDuration = 2f;
    public bool isInRange;
    [SerializeField] private Image suckTimerImage;

    public System.Action OnStartSuck;
    public System.Action OnStopSuck;
    public System.Action OnSucked;


    private void Start() {
        suckTimerImage.gameObject.SetActive(false);
    }

    private void Update() {

        if(isInRange)
        {
            // increase the timer if in range   
            suckTime += Time.deltaTime;

            //if timer reach the end call suck and reset timer
            if(suckTime >= suckDuration)
            {
                isInRange = false;
                OnSucked?.Invoke();
            }
        }
        else
        {
            //if out of range drecease the timer until 0
            if (suckTime > 0)
            {
                suckTime -= Time.deltaTime;
                
                //If timer reach 0 hide the visual image
                if(suckTime <= 0)
                {
                    suckTimerImage.gameObject.SetActive(false);
                }
            }
        }

        //update the visual
        suckTimerImage.fillAmount = (suckDuration - suckTime)/suckDuration;

    }

    //Object enter the SuckRange
     private void OnTriggerEnter(Collider other) {

        if (other.tag == "SuckRange")
        {
            isInRange = true;
            suckTimerImage.gameObject.SetActive(true);
            OnStartSuck?.Invoke();
        }
    }

    //Object exit the SuckRange
    private void OnTriggerExit(Collider other) {

        if (other.tag == "SuckRange")
        {
            isInRange = false;
            OnStopSuck?.Invoke();
        }
    }
}
