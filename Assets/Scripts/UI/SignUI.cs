using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SignUI : MonoBehaviour
{
    public static SignUI Instance;
    [SerializeField] private GameObject signUI;
    [SerializeField] private Image SignImage;
    [SerializeField] private TextMeshProUGUI signText;

    private void Start() {
        
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            gameObject.SetActive(false);
        }

        HideSign();
    }

    public void SetSign(string text, Sprite image)
    {
        signText.text = text;
        
        if(image != null)
        {
            SignImage.sprite = image;
            SignImage.gameObject.SetActive(true);
        }
        else
        {
            SignImage.gameObject.SetActive(false);
        }

        signUI.SetActive(true);
    }

    public void HideSign()
    {
        signUI.SetActive(false);
    }
}
