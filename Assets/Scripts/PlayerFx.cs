using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFx : MonoBehaviour
{
    [SerializeField] private GameObject smokeParticle;
    [SerializeField] private GameObject grassParticle;
    [SerializeField] private GameObject jumpFx;
    [SerializeField] private GameObject landingFx;
    [SerializeField] private GameObject dashFx;

    [SerializeField] private Transform smokeSpawnPoint;
    [SerializeField] private Transform grassSpawnPoint;

    [Space]
    [SerializeField] private AK.Wwise.Event onStepSFX;
    [SerializeField] private AK.Wwise.Event onDashSFX;
    [SerializeField] private AK.Wwise.Event onJumpSFX;
    [SerializeField] private AK.Wwise.Event onLandingSFX;
    [SerializeField] private AK.Wwise.Event onThrowSFX;
    [SerializeField] private AK.Wwise.Event onHitSFX;
    [SerializeField] private AK.Wwise.Event onSuckSFX;

    private PlayerController player;

    private void Start() {
        player = GetComponent<PlayerController>();
        player.OnDash += OnDashFx;
        player.OnJump += OnJumpFx;
        player.OnThrow += onThrowFX;
        player.OnHit += onHitFX;
        player.OnLanding += OnLandingFx;
        player.OnSuck += OnSuckFx;

    }

    private void OnDashFx()
    {

        PoolManager.Instantiate(dashFx,transform.position,transform.rotation);

        onDashSFX.Post(gameObject);
    }

    private void OnJumpFx()
    {
        if (jumpFx != null)
            {
                Instantiate(jumpFx, transform.position, Quaternion.identity);
                //PoolManager.Instance.Spawn(jumpFx,true,transform.position,Quaternion.identity);
            }

        onJumpSFX.Post(gameObject);
    }

    private void OnLandingFx()
    {
        PoolManager.Instantiate(landingFx, transform.position, Quaternion.identity);

        onLandingSFX.Post(gameObject);
    }

    public void Step()
    {
        Instantiate(smokeParticle, smokeSpawnPoint.position, Quaternion.identity);
        Instantiate(grassParticle, grassSpawnPoint.position, Quaternion.identity);

        onStepSFX.Post(gameObject);
    }

    private void OnSuckFx()
    {
        onSuckSFX.Post(gameObject);
    }

    private void onThrowFX()
    {
        onThrowSFX.Post(gameObject);
    }

    private void onHitFX(int _)
    {
        onHitSFX.Post(gameObject);
    }
}
