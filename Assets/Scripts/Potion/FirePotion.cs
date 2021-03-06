using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "FirePotion", menuName = "Potion/FirePotion", order = 0)]
public class FirePotion : Potion
{
    [Header("Fire Potion")]
    [SerializeField] LayerMask targetLayer;
    public override void OnExplosion(Transform target)
    {

        //Get Enemies in Range
        Collider[] enemiesCols = Physics.OverlapSphere(target.position,effectRange,targetLayer);

        foreach (Collider enemieCol in enemiesCols)
        {
            //enemieCol.GetComponent<Enemies>().Panic(useDuration);
            if(enemieCol.TryGetComponent<Enemies>(out Enemies enemy))
            {
                enemy.Panic(useDuration);
            }

        }



        //fx
        if (spawnFx != null)
        {
            PoolManager.Instance.Spawn(spawnFx,true,target.position,Quaternion.identity);
        }
    }

}
