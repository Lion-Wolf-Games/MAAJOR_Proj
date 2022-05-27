using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public DialogueSO test;
    void Start()
    {
        DialogueSystem.Instance.AddDialogueToQueue(test);
        
    }

}
