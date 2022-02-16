using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventCallback : MonoBehaviour
{
    public GameObject goTarget;

    private void Start()
    {
        if (goTarget == null)
        {
            goTarget = transform.parent.gameObject;
        }
    }

    public void Action(string message)
    {
        goTarget.SendMessage(message);
    }

}
