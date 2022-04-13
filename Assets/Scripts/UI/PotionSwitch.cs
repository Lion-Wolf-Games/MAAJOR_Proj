using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionSwitch : MonoBehaviour
{
    [SerializeField] public PlayerController playerControl;
    private PotionThrow potionThrow;

    [SerializeField] private Potion[] potionlist;
    [SerializeField] public int currentpotion;
    
    //True == next potion, False == PrevPotion
    public Action<bool> UpdateUI;


    private void Start() {

        potionThrow = playerControl.GetComponent<PotionThrow>();

        playerControl.NextPotion += NextPotion;
        playerControl.PrevPotion += PrevPotion;

        potionThrow.SetPotion(potionlist[currentpotion]);
    }

    private void NextPotion()
    {
        currentpotion++;

        if(currentpotion > potionlist.Length-1) 
            currentpotion = 0;

        potionThrow.SetPotion(potionlist[currentpotion]);

        UpdateUI?.Invoke(true);
    }

    private void PrevPotion()
    {
        currentpotion--;
        
        if(currentpotion <0) 
            currentpotion = potionlist.Length - 1;

        potionThrow.SetPotion(potionlist[currentpotion]);
        
        UpdateUI?.Invoke(false);
    }

    public Potion[] GetPotions()
    {
        return potionlist;
    }
}
