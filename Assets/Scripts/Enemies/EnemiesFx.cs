using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesFx : MonoBehaviour
{
    private Enemies enemy;

    [SerializeField] private GameObject onPlayerDetectedVfx;

    [SerializeField] private AK.Wwise.Event onAttackSfx;
    [SerializeField] private AK.Wwise.Event onHitSfx;
    [SerializeField] private AK.Wwise.Event onPlayerDetectedSfx;

    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemies>();

        enemy.OnAttack += OnAttackFx;
        enemy.OnHit += OnHitFx;
        enemy.OnPlayerDetected += OnPlayerDetectedFx;
    }

    private void OnPlayerDetectedFx()
    {
        PoolManager.Instance.Spawn(onPlayerDetectedVfx, true, transform.position, transform.rotation);
        onPlayerDetectedSfx.Post(gameObject);
    }

    private void OnHitFx()
    {
        onHitSfx.Post(gameObject);
    }

    private void OnAttackFx()
    {
        onAttackSfx.Post(gameObject);
        Debug.Log("Play attack fx");
    }
}
