using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : Interactable
{
    bool pickedUp = false;
    float timeBetween = 0.5f;
    float time = 0;
    Vector3 localPos;

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
            this.transform.parent = FindObjectOfType<PlayerScript>().playerCam.transform;
            localPos = this.transform.localPosition;
            pickedUp = true;
        }
    }

    void Reallise()
    {
        pickedUp = false;
        this.transform.parent = null;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        time = timeBetween;
        this.GetComponent<Rigidbody>().useGravity = true;
        this.gameObject.layer = LayerMask.NameToLayer("Default");
    }
}
