using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using DG.Tweening;

public class Door : MonoBehaviour
{
    [SerializeField] private List<Nest> linkedNest;
    public UnityEvent OnDoorOpen; 
    [Header("Visual")]
    [SerializeField] private Transform doorVisualOn;
    [SerializeField] private Transform doorVisualOff;
    [SerializeField] private Volume volume;
    [SerializeField] private float animationTime = 1f;

    [SerializeField] int nestRemain;
    [SerializeField] int nestTotal;

    public System.Action<float> UpdadeUI;

    private void Start() {
       InitialiseDoor(); 
    }

    private void InitialiseDoor()
    {
        foreach (Nest nest in linkedNest)
        {
            nest.OnNestDestroy += OnNestDestroyed;
        }

        nestRemain = linkedNest.Count;
        nestTotal = nestRemain;
    }

    private void OnNestDestroyed()
    {
        nestRemain --;

        if (nestRemain <= 0)
        {
            OpenDoor();
        }

        UpdadeUI?.Invoke(nestRemain / (float) nestTotal);
    }

    [ContextMenu("Close")]
    public void CloseDoor()
    {
        doorVisualOn.gameObject.SetActive(true);
        doorVisualOff.gameObject.SetActive(false);
        
        // doorVisual.DOShakePosition(animationTime,0.2f);
        // doorVisual.DOScaleY(1,animationTime).SetEase(Ease.OutCubic);
    }

    [ContextMenu("Open")]
    public void OpenDoor()
    {
        doorVisualOn.gameObject.SetActive(false);
        doorVisualOff.gameObject.SetActive(true);

        OnDoorOpen?.Invoke();

        // doorVisual.DOShakePosition(animationTime,0.2f);
        // doorVisual.DOScaleY(0,animationTime).SetEase(Ease.InExpo).OnComplete(()=>{doorVisual.gameObject.SetActive(false);});
    }

    private void OnDisable() {
        foreach (Nest nest in linkedNest)
        {
            nest.OnNestDestroy -= OnNestDestroyed;
        }
    }

    private void OnDrawGizmos() {
        if(linkedNest != null && linkedNest.Count >0)
        {
            Gizmos.color = Color.blue;

            foreach (var nest in linkedNest)
            {
                Gizmos.DrawLine(transform.position + Vector3.up,nest.transform.position);
            }

        }
    }
}
