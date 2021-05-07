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
    public Rigidbody playerRigid;
    public SterowanieRigidbody sterowanie;
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
    [HideInInspector]
    public bool pickedUpItem = false;

    [Header("Code Typing Management:")]
    public CodeHandler codeHandler;
    MAIPA.Interactable.Button backupButton = null;

    [Header("UI Elements:")]
    public GameObject interactableText;
    public GameObject itemIsNeededText;
    public Image inHandIMG;
    public GameObject inventoryUI;
    public GameObject pauseMenuUI;
    public GameObject codeUI;

    private void Start()
    {
        items = new List<ItemHandler>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (choosedItemID != -1)
        {
            bool found = false;
            foreach (var itm in items)
            {
                if (itm.GetID() == choosedItemID)
                {
                    SelectItem(choosedItemID, itm.GetImage());
                    found = true;
                    break;
                }
            }
            if (!found)
                choosedItemID = -1;
        }
    }

    void Update()
    {
        time -= Time.deltaTime;
        CheckRayHit();

        if (Input.GetKeyDown(KeyCode.Q) && time <= 0)
        {
            if(!pickedUpItem)
                DropItem();
        }

        // Open/Close Inventory
        if (Input.GetKeyDown(KeyCode.I) && time <= 0)
        {
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                sterowanie.active = true;
                playerRigid.constraints = RigidbodyConstraints.FreezeRotation;
            }
            else
            {
                inventoryUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                sterowanie.active = false;
                inventory.Refresh();
                time = betweenInputs;
                playerRigid.constraints = RigidbodyConstraints.FreezeAll;
            }
        }

        // Open/Close Pause Menu
        if(Input.GetKeyDown(KeyCode.Escape) && time <= 0)
        {
            if (codeUI.activeSelf)
            {
                codeUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                sterowanie.active = true;
                playerRigid.constraints = RigidbodyConstraints.FreezeRotation;

                backupButton = null;
            }
            else
            {
                if (pauseMenuUI.activeSelf)
                {
                    pauseMenuUI.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    sterowanie.active = true;
                    playerRigid.constraints = RigidbodyConstraints.FreezeRotation;
                }
                else
                {
                    pauseMenuUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    sterowanie.active = false;
                    time = betweenInputs;
                    playerRigid.constraints = RigidbodyConstraints.FreezeAll;
                }
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
                        ButtonCheck(btn);
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

    //Button:
    void ButtonCheck(MAIPA.Interactable.Button btn)
    {
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
                if (btn.isCoded)
                {
                    codeUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    sterowanie.active = false;
                    time = betweenInputs;
                    playerRigid.constraints = RigidbodyConstraints.FreezeAll;


                    codeHandler.codeType = btn.codeType;
                    if (btn.codeType == CodeType.TEXT_CODE)
                    {
                        codeHandler.SetTextCode(btn.textCode);
                    }
                    else if (btn.codeType == CodeType.NUM_CODE)
                    {
                        codeHandler.SetNumCode(btn.num1, btn.num2, btn.num3, btn.num4);
                    }
                    codeHandler.UpdateUI();
                    backupButton = btn;
                }
                else
                {
                    btn.Interact();
                }
            }
            else
            {
                if (!itemIsNeededText.activeSelf)
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
            if (btn.isCoded)
            {
                codeUI.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                sterowanie.active = false;
                time = betweenInputs;
                playerRigid.constraints = RigidbodyConstraints.FreezeAll;


                codeHandler.codeType = btn.codeType;
                if(btn.codeType == CodeType.TEXT_CODE)
                {
                    codeHandler.SetTextCode(btn.textCode);
                }
                else if(btn.codeType == CodeType.NUM_CODE)
                {
                    codeHandler.SetNumCode(btn.num1, btn.num2, btn.num3, btn.num4);
                }
                codeHandler.UpdateUI();
                backupButton = btn;
            }
            else
            {
                btn.Interact();
            }
        }
    }

    /// <summary>
    /// Used when code is typed corectly
    /// </summary>
    public void ButtonInteract()
    {
        codeUI.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        sterowanie.active = true;
        playerRigid.constraints = RigidbodyConstraints.FreezeRotation;

        backupButton.Interact();
        backupButton = null;
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
