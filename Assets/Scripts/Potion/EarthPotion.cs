using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[CreateAssetMenu(fileName = "EarthPotion", menuName = "Potion/EarthPotion", order = 0)]
public class EarthPotion : Potion
{
    [SerializeField] private GameObject wallTospawn;
    [SerializeField] private float timeActive;
    [SerializeField] private LayerMask layer;
    public override void OnExplosion(Transform target)
    {
        Physics.Raycast(target.position,Vector3.down,out RaycastHit hit,float.MaxValue,layer);


        GameObject wall = PoolManager.Instance.Spawn(wallTospawn,true,hit.point,Quaternion.identity);
        
        wall.GetComponent<EarthWall>().Spawn(timeActive);

        //fx
        if (spawnFx != null)
        {
            PoolManager.Instance.Spawn(spawnFx,true,target.position,Quaternion.identity);
        }
    }

}
