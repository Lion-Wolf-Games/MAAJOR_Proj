using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Potion : ScriptableObject {
    [SerializeField] protected UseType useType;
    [SerializeField] public float effectRange;
    [SerializeField] protected float useTime;
    [SerializeField] protected float useDuration;
    [SerializeField] public float launchRange;
    [SerializeField] public float cooldown;

    [Header("Fx")]
    [SerializeField] public GameObject spawnFx;
    [SerializeField] public GameObject trailFx;
    [SerializeField] public Material liquidMat;
    [SerializeField] public AK.Wwise.Event onCollisionSfx;



    public abstract void OnExplosion(Transform target);
}

public enum UseType
{
    Throw,
    Consomable
}
