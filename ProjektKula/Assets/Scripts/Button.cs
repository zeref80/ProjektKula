using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Interactable
{
    public bool interactableOnce = true;
    public UnityEvent thingsToHappen;
    int times = 0;

    public override void Interact()
    {
        if (interactableOnce && times > 0)
            return;

        thingsToHappen.Invoke();
        times++;
    }
}
