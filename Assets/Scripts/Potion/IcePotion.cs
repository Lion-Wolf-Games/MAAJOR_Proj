using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "IcePotion", menuName = "Potion/IcePotion", order = 0)]
public class IcePotion : Potion
{
    [Header("Ice Potion")]
    [SerializeField] LayerMask targetLayer;
    [SerializeField] GameObject iceBlock;
    public override void OnExplosion(Transform target)
    {

        //Get Enemies in Range
        Collider[] enemiesCols = Physics.OverlapSphere(target.position,effectRange,targetLayer);

        foreach (Collider enemieCol in enemiesCols)
        {
            //enemieCol.GetComponent<Enemies>().Panic(useDuration);
            if(enemieCol.TryGetComponent<Enemies>(out Enemies enemy))
            {
                enemy.StartCoroutine(enemy.TemporaryStopMovement(useDuration));
                PoolManager.Instance?.Spawn(iceBlock,true,enemy.transform.position,enemy.transform.rotation).transform.SetParent(enemy.transform);

            }

        }



        //fx
        if (spawnFx != null)
        {
            PoolManager.Instance.Spawn(spawnFx,true,target.position,Quaternion.identity);
        }
    }

}
