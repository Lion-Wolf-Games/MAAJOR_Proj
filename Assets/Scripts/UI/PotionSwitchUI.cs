using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using  DG.Tweening;

public class PotionSwitchUI : MonoBehaviour
{
    [SerializeField] private PotionSwitch potionSwitch;
    [SerializeField] private PotionThrow potionThrow;
    [SerializeField] private float mainScale;
    [SerializeField] private float sideScale;
    [SerializeField] private Image[] icons;
    [SerializeField] private Transform[] slots;

    bool isanimating;


    private void Start() {

        InitializeUI();
        potionSwitch.UpdateUI += SwitchPotion;
        potionThrow.OnThrowPotion += OnThrow;
    }

    private void InitializeUI()
    {
        SetIcons();
    }

    private void OnThrow(float cd)
    {
        Debug.Log("Throw UI");
        
        if(isanimating) return;

        isanimating = true;
        slots[0].GetChild(0).DOPunchScale(Vector3.one * 0.2f,0.2f).OnComplete(() => {isanimating = false;});
        
        SetCooldown(cd);
    }

    private void SetCooldown(float cooldown)
    {
        
        Image cdImage = slots[0].GetChild(0).GetChild(0).GetComponent<Image>();

        cdImage.gameObject.SetActive(true);
        cdImage.fillAmount = 1;
        cdImage.DOFillAmount(0,cooldown).SetEase(Ease.Linear).OnComplete(() => {cdImage.gameObject.SetActive(false);});
    }

    private void SwitchPotion(bool state)
    {
        if(state)
        {
            NextPotion();
        }
        else
        {
            PrevPotion();
        }
    }

    private void NextPotion()
    {
        for (int i = slots.Length - 1; i >= 0 ; i--)
        {
            int targetSlot = (i - 1)% slots.Length;

            if(targetSlot <0)
                targetSlot = slots.Length -1;

            slots[i].GetChild(0).DOMove(slots[targetSlot].position,0.1f);
            slots[i].GetChild(0).SetParent(slots[targetSlot]);
        }

        slots[0].GetChild(0).GetComponent<RectTransform>().DOSizeDelta(Vector2.one * mainScale,0.2f);
        slots[1].GetChild(0).GetComponent<RectTransform>().DOSizeDelta(Vector2.one * sideScale,0.2f);
        slots[3].GetChild(0).GetComponent<RectTransform>().DOSizeDelta(Vector2.one * sideScale,0.2f);

    }

    private void PrevPotion()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].GetChild(0).DOMove(slots[(i + 1)% slots.Length].position,0.1f);
            slots[i].GetChild(0).SetParent(slots[(i + 1)% slots.Length]);
        }

        slots[0].GetChild(0).GetComponent<RectTransform>().DOSizeDelta(Vector2.one * mainScale,0.2f);
        slots[1].GetChild(0).GetComponent<RectTransform>().DOSizeDelta(Vector2.one * sideScale,0.2f);
        slots[3].GetChild(0).GetComponent<RectTransform>().DOSizeDelta(Vector2.one * sideScale,0.2f);

    }


    private void SetIcons()
    {
        Potion[] selectedPotions = potionSwitch.GetPotions();


        for (int i = 0; i < slots.Length; i++)
        {
            icons[i].sprite = selectedPotions[(potionSwitch.currentpotion + i)%selectedPotions.Length].Icon;
            
            //Set fill amount of cooldown to 0
            icons[i].transform.GetChild(0).GetComponent<Image>().fillAmount = 0;
            
            //hide coolDown Image
            icons[i].transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
