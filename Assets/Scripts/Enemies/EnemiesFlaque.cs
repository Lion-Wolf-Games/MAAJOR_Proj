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

    private SuckableObject suckableObject;

    private void Start() {
        suckableObject = GetComponent<SuckableObject>();

        suckableObject.OnStartSuck += OnStartSuck;
        suckableObject.OnStopSuck += OnStopSuck;
        suckableObject.OnSucked += OnSuck;
    }


    private void OnSuck()
    {
        //GetPlayer
        PlayerController player = FindObjectOfType<PlayerController>();

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
        else //If no enemie respawn
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

    private void OnStartSuck()
    {
        //slow the enemy while sucking
        if(enemie != null)
        {
            enemie.SetSpeedMul(0.5f);
        }
    }
    private void OnStopSuck()
    {
        //reset the enemy speed
        if(enemie != null)
        {
            enemie.SetSpeedMul(1);
        }
    }
}
