using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAIPA.Interactable
{
    public class Item : Interactable
    {
        public int itemID;
        public string itemName;
        public Sprite itemImage;
        [TextArea]
        public string itemInfo;

        public override void Interact()
        {
            Destroy(this.gameObject);
        }

        public void SetVariables(int id, string name, Sprite image, string info)
        {
            itemID = id;
            itemName = name;
            itemImage = image;
            itemInfo = info;
        }
    }
}
