using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MAIPA.Interactable
{
    public class PickUp : Interactable
    {
        bool pickedUp = false;
        float timeBetween = 0.1f;
        float time = 0;
        Vector3 localPos;
        Vector3 pos;
        float distance;

        private void Update()
        {
            if (pickedUp)
            {
                transform.localPosition = localPos;
                if (Input.GetKeyDown(KeyCode.E))
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
                distance = Vector3.Distance(FindObjectOfType<PlayerScript>().playerCam.transform.position, this.transform.position);
                this.transform.parent = FindObjectOfType<PlayerScript>().playerCam.transform;
                localPos = this.transform.localPosition;
                pickedUp = true;
            }
        }

        void Reallise()
        {
            pickedUp = false;
            pos = transform.position;
            this.transform.parent = null;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            time = timeBetween;
            this.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.layer = LayerMask.NameToLayer("Default");
            //this.transform.position = FindObjectOfType<PlayerScript>().playerCam.transform.position + FindObjectOfType<PlayerScript>().playerCam.transform.forward * distance;
            transform.position = pos;
        }
    }
}
