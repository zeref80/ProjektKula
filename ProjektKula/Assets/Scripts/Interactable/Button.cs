using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MAIPA.Interactable
{
    public class Button : Interactable
    {
        public bool interactableOnce = true;
        public UnityEvent thingsToHappen;
        int times = 0;

        public bool isItemNeeded = false;
        [Header("Only if item is Needed:")]
        public List<int> itemIds = new List<int>();

        public override void Interact()
        {
            if (interactableOnce && times > 0)
                return;

            thingsToHappen.Invoke();
            times++;
        }
    }
}
