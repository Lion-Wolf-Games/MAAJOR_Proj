using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public Midena midena;
    public Transform pos1;
    public void FirstTrigger()
    {
        DialogueSystem.Instance._onDialogueEnd += MidenaToPos1;
    }

    private void MidenaToPos1()
    {
        midena.MoveToTarget(pos1);
    }
}
