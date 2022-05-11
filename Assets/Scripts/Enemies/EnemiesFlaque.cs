using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EnemiesFlaque : MonoBehaviour
{
    //private PlayerController player;
    [SerializeField] private GameObject suckFx;
    [SerializeField] private GameObject deathFx;

    [SerializeField] private Enemies enemie;

    [SerializeField] private float suckTime;
    [SerializeField] private float suckDuration = 2f;
    [SerializeField] private bool isInRange;
    [SerializeField] private Image suckTimerImage;

    // private void OnTriggerEnter(Collider other) {
    //     if (other.tag == "Player")
    //     {
    //         player = other.GetComponent<PlayerController>();
    //         player.OnSuck += OnSuck;
    //     }
    // }

    private void Start() {
        suckTimerImage.gameObject.SetActive(true);
    }

    private void Update() {

        if(isInRange)
        {
            suckTime += Time.deltaTime;
            if(suckTime >= suckDuration)
            {
                isInRange = false;
                OnSuck(FindObjectOfType<PlayerController>());
            }
        }
        else
        {
            if (suckTime > 0)
            {
                suckTime -= Time.deltaTime;

                if(suckTime <= 0)
                {
                    suckTimerImage.gameObject.SetActive(false);
                }
            }
        }

        suckTimerImage.fillAmount = (suckDuration - suckTime)/suckDuration;
    }

    private void OnSuck(PlayerController player)
    {
        //Spawn Fx
        GameObject fxgo =  PoolManager.Instance.Spawn(suckFx,true,transform.position,transform.rotation);
        //Set Fx to go to pim
        float travelDuration = Random.Range(0.9f,1.1f);

        fxgo.transform.DOJump(player.suckPosition.position,2.5f,1,travelDuration)
            .SetEase(Ease.InOutCubic);
        //Spawn DeathFx
        PoolManager.Instance.Spawn(deathFx,true,transform.position,transform.rotation);
        //Destroy
        
        if(enemie != null)
        {
            enemie.Kill();
        }
        else
        {
            gameObject.SetActive(false);
            DebugRespawn();
        }

        //player.OnSuck -= OnSuck;
    }

    private async void DebugRespawn()
    {
        await System.Threading.Tasks.Task.Delay(2500);
        gameObject.SetActive(true);
    }


    public void InRange(bool state)
    {
        isInRange = state;

        if (isInRange)
        {
            suckTimerImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "SuckRange")
        {
            isInRange = true;
            suckTimerImage.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other) {

        if (other.tag == "SuckRange")
        {
            isInRange = false;
        }
    }

    // private void OnDisable() {
    //     if (player!=null)
    //     {
    //         player.OnSuck -= OnSuck;
    //     }
    // }
}
