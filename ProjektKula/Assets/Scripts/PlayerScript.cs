using MyBox;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MAIPA.Interactable;
using System.Net;

/// <summary>
/// This is main Player Script
/// </summary>
public class PlayerScript : MonoBehaviour
{
    public Camera playerCam;
    public Sterowanie sterowanie;
    public LayerMask raycastLayer;
    public float rayDistance = 3f;

    float betweenInputs = 0.1f;
    float time = 0;

    [Header("Items Management:")]
    public List<ItemHandler> items;
    [HideInInspector]
    public int choosedItemID = -1;
    public ItemDatabase itemDatabase;
    public Transform handTransform;
    GameObject objectInHand;
    public Inventory inventory;

    [Header("UI Elements:")]
    public GameObject interactableText;
    public Image inHandIMG;
    public GameObject inventoryUI;
    public GameObject itemIsNeededText;

    private void Start()
    {
        items = new List<ItemHandler>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (choosedItemID != -1)
        {
            foreach (var itm in items)
            {
                if (itm.GetID() == choosedItemID)
                {
                    SelectItem(choosedItemID, itm.GetImage());
                }
            }
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        CheckRayHit();

        if (Input.GetKeyDown(KeyCode.Q) && time <= 0)
        {
            DropItem();
        }

        // Open\Close Inventory
        if (Input.GetKeyDown(KeyCode.I) && time <= 0)
        {
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                sterowanie.active = true;
            }
            else
            {
                inventoryUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                sterowanie.active = false;
                inventory.Refresh();
                time = betweenInputs;
            }
        }
    }

    void CheckRayHit()
    {
        RaycastHit hit;
        if(Physics.Raycast(playerCam.transform.position, playerCam.transform.forward, out hit, rayDistance, raycastLayer))
        {
            if (hit.collider.GetComponent<Interactable>() != null)
            {
                if(hit.collider.GetComponent<MAIPA.Interactable.Button>() != null)
                {
                    if (!hit.collider.GetComponent<MAIPA.Interactable.Button>().isInteractable())
                        return;
                }

                interactableText.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.collider.GetComponent<Item>() != null)
                    {
                        PickupItem(hit.collider.GetComponent<Item>());
                        hit.collider.GetComponent<Interactable>().Interact();
                    }
                    else if (hit.collider.GetComponent<MAIPA.Interactable.Button>() != null)
                    {
                        MAIPA.Interactable.Button btn = hit.collider.GetComponent<MAIPA.Interactable.Button>();
                        if (btn.isItemNeeded)
                        {
                            bool isId = false;
                            foreach (var id in btn.itemIds)
                            {
                                if (choosedItemID == id)
                                {
                                    isId = true;
                                    break;
                                }
                            }

                            if (isId)
                            {
                                hit.collider.GetComponent<Interactable>().Interact();
                            }
                            else
                            {
                                if (!itemIsNeededText.active)
                                    itemIsNeededText.SetActive(true);
                                else
                                {
                                    itemIsNeededText.SetActive(false);
                                    itemIsNeededText.SetActive(true);
                                }
                            }
                        }
                        else
                        {
                            hit.collider.GetComponent<Interactable>().Interact();
                        }
                    }
                    else
                    {
                        hit.collider.GetComponent<Interactable>().Interact();
                    }
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

    //Items:
    (ItemHandler, bool) HasItem(Item item)
    {
        foreach(var itm in items)
        {
            if(itm.GetID() == item.itemID)
            {
                return (itm, true);
            }
        }
        return (null, false);
    }

    void PickupItem(Item backup)
    {
        (ItemHandler, bool) itm = HasItem(backup);
        if (itm.Item2)
        {
            itm.Item1.AddItem(1);
        }
        else
        {
            ItemHandler baseItem = new ItemHandler(backup.itemID, backup.itemName, backup.itemImage, backup.itemInfo);
            items.Add(baseItem);

            if (choosedItemID == -1)
            {
                SelectItem(baseItem.GetID(), baseItem.GetImage());
            }
        }
    }

    void DropItem()
    {
        foreach (var itm in items)
        {
            if (itm.GetID() == choosedItemID)
            {
                GameObject itemOnScene = Instantiate(itemDatabase.itemPrefabs[choosedItemID], playerCam.transform, true);
                itemOnScene.transform.position = playerCam.transform.position + playerCam.transform.forward * 7f;
                itemOnScene.transform.parent = null;
                itemOnScene.GetComponent<Item>().SetVariables(itm.GetID(), itm.GetName(), itm.GetImage(), itm.GetInfo());
                itm.AddItem(-1);

                if (itm.GetNum() == 0)
                {
                    items.Remove(itm);
                    SelectItem(-1);
                }
                break;
            }
        }
    }

    public void SelectItem(int id, Sprite image = null)
    {
        if(objectInHand != null)
        {
            Destroy(objectInHand);
            objectInHand = null;
        }

        if(choosedItemID != -1)
        {
            foreach(var item in inventory.itemUIs)
            {
                if(item.itemID == choosedItemID)
                {
                    item.selectedText.SetActive(false);
                }
            }
        }

        choosedItemID = id;
        if(id == -1)
        {
            inHandIMG.sprite = null;
        }
        else
        {
            inHandIMG.sprite = image;
            objectInHand = Instantiate(itemDatabase.itemPrefabs[choosedItemID], handTransform);
            if (objectInHand.GetComponent<Rigidbody>() != null)
            {
                objectInHand.GetComponent<Rigidbody>().useGravity = false;
                objectInHand.GetComponent<Rigidbody>().isKinematic = true;
                objectInHand.GetComponent<Rigidbody>().freezeRotation = true;
            }
            objectInHand.transform.localScale = itemDatabase.itemInHandSize[choosedItemID];
        }
    }

    public Item GetItem(int id)
    {
        return itemDatabase.itemPrefabs[id].GetComponent<Item>();
    }
}
