using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public enum HealthBarTypes
    {
        Fill,
        Sprites
    }
    public HealthBarTypes healthBarType;

    [SerializeField] private GameObject _healthPannel;
    [SerializeField] private int currentValue;

    [Header("Fill")]
    [SerializeField] private Slider _healthBar;

    [SerializeField] private Slider _ghostBar;
    [SerializeField] private float _ghostBarSpeed;

    [Header("Sprites")]
    [SerializeField] private Image healthImage;
    [SerializeField] private Sprite[] healthSprites;

    [Header("Portrait")]
    [SerializeField] private Image portraitImage;
    [SerializeField] private Sprite[] portraitSprites;
    
    public bool _isWorldSpace;

    public void SetMax(int maxValue)
    {
        currentValue = maxValue;

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
        if (value < currentValue && portraitImage != null)
        {
            StartCoroutine(SpriteTimedChange());
            portraitImage.transform.DOShakePosition(0.5f, 10);
        }

        currentValue = value;

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

    IEnumerator SpriteTimedChange()
    {
        portraitImage.sprite = portraitSprites[1];
        yield return new WaitForSeconds(0.5f);
        portraitImage.sprite = portraitSprites[0];
    }

    private void LateUpdate()
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
