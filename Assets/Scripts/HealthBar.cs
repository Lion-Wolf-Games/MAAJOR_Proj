using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject _healthPannel;

    [SerializeField] private Slider _healthBar;

    [SerializeField] private Slider _ghostBar;
    [SerializeField] private float _ghostBarSpeed;
    
    public bool _isWorldSpace;

    public void SetMax(int maxValue)
    {
        _healthBar.maxValue = maxValue;
        _ghostBar.maxValue = maxValue;

        UpdateValue(maxValue);

        if (_isWorldSpace)
        {
            _healthPannel.SetActive(false);
        }
    }

    public void UpdateValue(int value)
    {
        if (_isWorldSpace)
        {
            _healthPannel.SetActive(true);
        }

        _healthBar.value = value;
    }

    private void Update()
    {
        if (_isWorldSpace)
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        if (_ghostBar.value != _healthBar.value)
        {
            _ghostBar.value = Mathf.Lerp(_ghostBar.value, _healthBar.value, _ghostBarSpeed * Time.deltaTime);
        }
    }
}
