using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EarthWall : LivingObject
{
    [SerializeField] private float spawnTime;
    [SerializeField] private GameObject destroyFx;
    [SerializeField] private GameObject spawnFx;
    private float destroyTime;
    private bool isWallUp;

    static EarthWall currentWall;

    public void Spawn(float time)
    {
        if (currentWall != null)
        {
            currentWall.WallDown();
        }

        currentWall = this;

        //reset Scale
        transform.localScale = new Vector3(1,0,1);

        //Spawn Fx + start Scale + shake
        //PoolManager.Instance.Spawn(spawnFx,true,transform.position,transform.rotation);
        transform.DOScaleY(1,spawnTime);
        transform.DOShakePosition(spawnTime,0.2f);
        isWallUp = true;

        destroyTime = Time.time + time;
    }

    private void Update() 
    {
        /*if (isWallUp && Time.time >= destroyTime)
        {
            isWallUp = false;
            transform.DOShakePosition(0.2f,0.5f);
            transform.DOMoveY(transform.position.y - 5f,0.5f).OnComplete( () => {gameObject.SetActive(false);});
        }*/
    }

    public void WallDown()
    {
        isWallUp = false;
        transform.DOShakePosition(0.2f, 0.5f);
        transform.DOMoveY(transform.position.y - 5f, 0.5f).OnComplete(() => { gameObject.SetActive(false); });
    }

}
