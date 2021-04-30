using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace MAIPA.Interactable
{
    public enum CodeType
    {
        TEXT_CODE,
        NUM_CODE
    }

    public class Button : Interactable
    {
        public bool notInteractable = false;

        public bool interactableOnce = true;
        public UnityEvent thingsToHappen;
        int times = 0;

        public bool isItemNeeded = false;
        public List<int> itemIds = new List<int>();

        [Space(15)]
        public bool isCoded = false;
        [ConditionalField("isCoded", compareValues: true)]
        public CodeType codeType;
        [ConditionalField("isCoded", compareValues: true)]
        public string textCode;
        [ConditionalField("isCoded", compareValues: true)]
        public int num1;
        [ConditionalField("isCoded", compareValues: true)]
        public int num2;
        [ConditionalField("isCoded", compareValues: true)]
        public int num3;
        [ConditionalField("isCoded", compareValues: true)]
        public int num4;

        public override void Interact()
        {
            if (interactableOnce && times > 0 || notInteractable)
                return;

            thingsToHappen.Invoke();
            times++;
        }

        public bool isInteractable()
        {
            if ((interactableOnce && times > 0) || notInteractable)
                return false;
            return true;
        }
    }
}
