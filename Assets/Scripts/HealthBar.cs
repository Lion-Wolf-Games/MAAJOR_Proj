using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public enum HealthBarTypes
    {
        Fill,
        Sprites
    }
    public HealthBarTypes healthBarType;

    [SerializeField] private GameObject _healthPannel;


    [Header("Fill")]
    [SerializeField] private Slider _healthBar;

    [SerializeField] private Slider _ghostBar;
    [SerializeField] private float _ghostBarSpeed;

    [Header("Sprites")]
    [SerializeField] private Image healthImage;
    [SerializeField] private Sprite[] healthSprites;
    
    public bool _isWorldSpace;

    public void SetMax(int maxValue)
    {
        switch (healthBarType)
        {
            case HealthBarTypes.Fill:

                _healthBar.maxValue = maxValue;
                _ghostBar.maxValue = maxValue;

                UpdateValue(maxValue);

                break;
            case HealthBarTypes.Sprites:
                break;
            default:
                break;
        }

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

        switch (healthBarType)
        {
            case HealthBarTypes.Fill:

                _healthBar.value = value;

                break;
            case HealthBarTypes.Sprites:

                if (healthSprites.Length > value && value >= 0)
                {
                    healthImage.sprite = healthSprites[value];
                }

                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (_isWorldSpace)
        {
            transform.rotation = Camera.main.transform.rotation;
        }

        if (healthBarType == HealthBarTypes.Fill)
        {
            if (_ghostBar.value != _healthBar.value)
            {
                _ghostBar.value = Mathf.Lerp(_ghostBar.value, _healthBar.value, _ghostBarSpeed * Time.deltaTime);
            }
        }    
    }
}
