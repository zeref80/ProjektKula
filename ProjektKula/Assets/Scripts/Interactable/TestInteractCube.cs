using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAIPA.Interactable
{
    public class TestInteractCube : Interactable
    {
        public override void Interact()
        {
            Debug.Log("Interacted with " + this.name);
        }
    }
}
