using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueGO : MonoBehaviour
{
    [SerializeField] private DialogueSO dialogueSO;
    public UnityEvent OnDialogueEnd;

    // Start is called before the first frame update
    void Start()
    {
        DialogueSystem.Instance.AddDialogueToQueue(dialogueSO);

        if(OnDialogueEnd != null)
        {
            SubscribeEvents();
        }
    }

    private void SubscribeEvents()
    {
        
        DialogueSystem.Instance._onDialogueEnd += OnDialogueEnd.Invoke;
        DialogueSystem.Instance._onDialogueEnd += UnSubscribeEvents;
    }

    private void UnSubscribeEvents()
    {
        DialogueSystem.Instance._onDialogueEnd -= OnDialogueEnd.Invoke;
        DialogueSystem.Instance._onDialogueEnd -= UnSubscribeEvents;
    }
}
