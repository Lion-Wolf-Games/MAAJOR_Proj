using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PotionBehavior : MonoBehaviour
{
    [SerializeField] Potion potionType;
    [SerializeField] Transform potionTrailParent;
    [SerializeField] MeshRenderer liquidMesh;

    public void SetUpPotion(Potion potion)
    {
        potionType = potion;


        //Clear Previous fx
        for (int i = potionTrailParent.childCount - 1; i >= 0 ; i--)
        {
            Destroy(potionTrailParent.GetChild(i));
        }

        //SpawnTrailFx
        if (potion.trailFx != null)
        {
            Instantiate(potion.trailFx,potionTrailParent);  
        }

        //ChangePotionColor
        liquidMesh.material = potion.liquidMat;
    }

    private void OnCollisionEnter(Collision other) {
        potionType.OnExplosion(transform);

        //Clear Fx
        for (int i = potionTrailParent.childCount - 1; i >= 0 ; i--)
        {
            Destroy(potionTrailParent.GetChild(i).gameObject);
        }

        CinemachineCamshake.Instance.ShakeCam(1,0.2f);

        gameObject.SetActive(false);
    }
}
