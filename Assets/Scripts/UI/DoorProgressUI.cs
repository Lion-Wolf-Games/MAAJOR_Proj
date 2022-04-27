using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class DoorProgressUI : MonoBehaviour
{
    [SerializeField] private Door targetDoor;

    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI percentageText;

    private void Start() {
        targetDoor.UpdadeUI += UpdadeUI;
        UpdadeUI(1);
    }

    private void UpdadeUI(float value)
    {
        fillImage.DOFillAmount(1-value,0.2f);

        percentageText.text = ((1-value) * 100).ToString() + "%";
    }
}
