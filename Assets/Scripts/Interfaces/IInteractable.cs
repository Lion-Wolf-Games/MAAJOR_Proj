using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool canBePickedUp { get; set; }

    public void OnInteract();
}
