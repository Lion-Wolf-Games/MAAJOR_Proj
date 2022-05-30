using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LivingObject : MonoBehaviour, IDamagable
{
    public int _health;
    public int _maxHealth;

    [SerializeField] private HealthBar _healthBar;
    private bool _hasHealthBar;

    [Header("Invicibility Frames")]
    [SerializeField] bool _canGetHit = true;
    [SerializeField] float _invincibilityTime;
    private IEnumerator _currentTimer;

    protected bool _isAlive = true;
    public Action<GameObject> OnKill;
    public Action<int> OnHealthChange;
    public Action<int> OnHit;
    public Action<int> OnHeal;

    [Header("Animation")]
    [SerializeField] protected Animator animator;
    [SerializeField] private GameObject _deathFx;
    [SerializeField] private GameObject _hitFx;
    [SerializeField] private Transform _hitSpawnPos;
    [SerializeField] protected bool _canBeInterupted = true;

    protected virtual void Awake()
    {
        if (_health == 0)
        {
            _health = _maxHealth;
        }

        if (GetComponentInChildren<HealthBar>() != null && _healthBar == null)
        {
            _healthBar = GetComponentInChildren<HealthBar>();

            _hasHealthBar = true;
            _healthBar.SetMax(_maxHealth);
        }
        else if (_healthBar != null)
        {
            _hasHealthBar = true;
            _healthBar.SetMax(_maxHealth);
        }

        animator = GetComponentInChildren<Animator>();

    }

    public virtual void ChangeHealth(int value)
    {
        ChangeHealth(value,transform.position);
    }
    public virtual void ChangeHealth(int hP, Vector3 origin) //Heal or Hurt
    {

        if (!_isAlive)
        {
            return;
        }

        if (hP > 0)  //Heal
        {
            Heal(hP);
        }
        else if (_canGetHit)   //Hurt
        {
            Hit(hP, origin);
        }

        if (_hasHealthBar)
        {
            UpdateHealthBar();
        }
    }

    protected virtual void Hit(int damage , Vector3 origin)
    {
        _health = Mathf.Clamp(_health + damage, 0, _maxHealth);

            PoolManager.Instance.Spawn(_hitFx, true, _hitSpawnPos.position, _hitSpawnPos.rotation);

            if (_health == 0)
            {
                if (_hasHealthBar)
                {
                    UpdateHealthBar();
                }

                Death();
                

                return;
            }

            if (animator != null && _canBeInterupted)
            {
                animator.SetTrigger("Hit");
            }

            OnHealthChange?.Invoke(_health);
            InvincibilityFrames();
    }

    protected virtual void Heal(int healAmmount)
    {
        Debug.Log("Heal");

            _health = Mathf.Clamp(_health + healAmmount, 0, _maxHealth);

            if (_hasHealthBar)
            {
                UpdateHealthBar();
            }

            OnHealthChange?.Invoke(_health);
            OnHeal?.Invoke(_health);
    }

    private void UpdateHealthBar()
    {
        _healthBar.UpdateValue(_health);
    }

    virtual protected void Death()
    {
        Debug.Log(gameObject.name + " died");

        GetComponent<Collider>().enabled = false;
        _canGetHit = false;

        _isAlive = false;
        StartCoroutine(DeathCoroutine(1.5f));

        animator.SetTrigger("Death");

        OnKill?.Invoke(gameObject);
    }

    protected IEnumerator DeathCoroutine(float delay)
    {
        if (_deathFx != null)
        {
            PoolManager.Instance.Spawn(_deathFx, true, transform.position + Vector3.up * 0.5f, transform.rotation);
        }

        yield return null;
    }

    public void SetInvulnerable(bool state)
    {
        StartCoroutine("SetInvulnerableCr",state);
    }

    IEnumerator SetInvulnerableCr(bool state)
    {
        yield return new WaitForEndOfFrame();
        _canGetHit = !state;

        StopCoroutine(_currentTimer);
    }

    #region InvicibilityFrames
    private void InvincibilityFrames()
    {
        _canGetHit = false;

        _currentTimer = Timer();

        StartCoroutine(_currentTimer);
    }

    private IEnumerator Timer()
    {
        yield return new WaitForSeconds(_invincibilityTime);

        _canGetHit = true;
    }

    #endregion
}
