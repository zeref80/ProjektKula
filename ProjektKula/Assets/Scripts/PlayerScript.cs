using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public Camera playerCam;
    public LayerMask raycastLayer;

    [Header("UI Elements:")]
    public GameObject interactableText;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckRayHit();
    }

    void CheckRayHit()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, 3f, raycastLayer))
        {
            if (hit.collider.GetComponent<Interactable>() != null)
            {
                interactableText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.GetComponent<Interactable>().Interact();
                }
            }
            else
            {
                interactableText.SetActive(false);
            }
        }
        else
        {
            interactableText.SetActive(false);
        }
    }
}
