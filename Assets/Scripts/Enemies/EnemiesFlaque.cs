using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemiesFlaque : MonoBehaviour
{
    private PlayerController player;
    [SerializeField]private GameObject suckFx;
    [SerializeField]private GameObject deathFx;



    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player")
        {
            player = other.GetComponent<PlayerController>();
            player.OnSuck += OnSuck;
        }
    }

    private void OnSuck()
    {
        //Spawn Fx
        GameObject fxgo =  PoolManager.Instance.Spawn(suckFx,true,transform.position,transform.rotation);
        //Set Fx to go to pim
        float travelDuration = Random.Range(0.9f,1.1f);

        fxgo.transform.DOJump(player.suckPosition.position,5,1,travelDuration)
            .SetEase(Ease.InOutCubic);
        //Spawn DeathFx
        PoolManager.Instance.Spawn(deathFx,true,transform.position,transform.rotation);
        //Destroy
        gameObject.SetActive(false);
        DebugRespawn();
        player.OnSuck -= OnSuck;
    }

    private async void DebugRespawn()
    {
        await System.Threading.Tasks.Task.Delay(2500);
        gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other) {

        if (other.tag == "Player")
        {
            player.OnSuck -= OnSuck;
        }
    }

    private void OnDisable() {
        if (player!=null)
        {
            player.OnSuck -= OnSuck;
        }
    }
}
