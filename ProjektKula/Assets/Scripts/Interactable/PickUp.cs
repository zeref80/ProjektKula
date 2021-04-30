using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAIPA.Interactable
{
    [RequireComponent(typeof(Rigidbody))]
    public class PickUp : Interactable
    {
        bool pickedUp = false;
        float timeBetween = 0.2f;
        float time = 0;
        Vector3 localPos;
        Vector3 pos;
        Transform prevParent;

        private void Update()
        {
            if (pickedUp)
            {
                transform.localPosition = localPos;
                FindObjectOfType<PlayerScript>().pickedUpItem = true;
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    Reallise();
                }
            }

            time -= Time.deltaTime;
        }

        public override void Interact()
        {
            if (time <= 0)
            {
                this.GetComponent<Rigidbody>().useGravity = false;
                this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                prevParent = this.transform.parent;
                this.transform.parent = null;
                this.transform.parent = FindObjectOfType<PlayerScript>().playerCam.transform;
                localPos = this.transform.localPosition;
                pickedUp = true;
            }
        }

        void Reallise()
        {
            pickedUp = false;
            FindObjectOfType<PlayerScript>().pickedUpItem = false;
            pos = transform.position;
            this.transform.parent = null;
            this.transform.parent = prevParent;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            time = timeBetween;
            this.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.layer = LayerMask.NameToLayer("Default");
            //this.transform.position = FindObjectOfType<PlayerScript>().playerCam.transform.position + FindObjectOfType<PlayerScript>().playerCam.transform.forward * distance;
            transform.position = pos;
        }
    }
}
