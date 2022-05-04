using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "WindPotion", menuName = "Potion/WindPotion", order = 0)]
public class WindPotion : Potion
{
    [SerializeField] LayerMask targetLayer;
    [SerializeField] float pushDistance;
    public override void OnExplosion(Transform target)
    {

        //Get Enemies in Range
        Collider[] enemiesCols = Physics.OverlapSphere(target.position,effectRange,targetLayer);

        foreach (Collider enemieCol in enemiesCols)
        {
            Vector3 dir = enemieCol.transform.position - target.position;
            dir.y = 0;
            dir.Normalize();

            Vector3 endPos;
            RaycastHit raycastHit;

            //Check pour pas faire passer les ennemies a travers un mur
            if (Physics.Raycast(enemieCol.transform.position + Vector3.up ,dir,out raycastHit,pushDistance))
            {
                endPos = raycastHit.point - dir * 0.1f;
                endPos.y = enemieCol.transform.position.y;
            }
            else
            {
                endPos = enemieCol.transform.position + dir * pushDistance;
            }

            //Pousser les ennemies
            enemieCol.transform.DOMove(endPos,0.5f);
        }

        //fx
        if (spawnFx != null)
        {
            PoolManager.Instance.Spawn(spawnFx,true,target.position,Quaternion.identity);
        }

        //Get Platform in Range
        Collider[] platforms = Physics.OverlapSphere(target.position,effectRange);

        foreach(Collider col in platforms)
        {
            if (col.TryGetComponent<PushableStone>(out PushableStone stone))
            {
                stone.Push(target.position);
            }
            else
            {
                continue;
            }
        }
    }

}
