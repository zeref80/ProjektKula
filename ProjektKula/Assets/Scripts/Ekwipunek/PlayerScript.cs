using MyBox;
using NUnit.Framework.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MAIPA.Interactable;

/// <summary>
/// This are states for player
/// </summary>
public enum PlayerState {InInvetory, Paused, Playing, Decoding}

/// <summary>
/// This is main Player Script
/// </summary>
public class PlayerScript : MonoBehaviour
{
    [HideInInspector]
    public PlayerState playerState;
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
    public int selectedSlot = 0;
    public int[] choosedItemsID = { -1, -1, -1, -1 };
    public ItemDatabase itemDatabase;
    public Transform handTransform;
    GameObject objectInHand;
    public Inventory inventory;
    [HideInInspector]
    public bool pickedUpItem = false;

    [Header("Code Typing Management:")]
    public CodeHandler codeHandler;
    private MAIPA.Interactable.Button backupButton = null;

    [Header("UI Elements:")]
    public GameObject interactableText;
    public GameObject itemIsNeededText;
    public Image[] slotsIMG = { null, null, null, null };
    public Image[] slotsBgIMG = { null, null, null, null };
    public Color selectedSlotColor;
    public Color notSelectedSlotColor;
    public GameObject inventoryUI;
    public GameObject pauseMenuUI;
    public GameObject codeUI;

    private void Start()
    {
        items = new List<ItemHandler>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for(int i = 0; i < choosedItemsID.Length; i++)
        {
            if (choosedItemsID[i] != -1)
            {
                bool found = false;
                foreach (var itm in items)
                {
                    if (itm.GetID() == choosedItemsID[i])
                    {
                        SelectItem(selectedSlot,choosedItemsID[i], itm.GetImage());
                        found = true;
                        break;
                    }
                }
                if (!found)
                    choosedItemsID[i] = -1;
            }
        }

        RefreshSelectedSlot();

        playerState = PlayerState.Playing;
    }

    void Update()
    {
        time -= Time.deltaTime;
        CheckRayHit();

        if (Input.GetKeyDown(KeyCode.Q) && time <= 0 && playerState == PlayerState.Playing)
        {
            if(!pickedUpItem)
                DropItem();
        }

        // Open/Close Inventory
        if (Input.GetKeyDown(KeyCode.I) && time <= 0 && (playerState == PlayerState.Playing || playerState == PlayerState.InInvetory))
        {
            if (inventoryUI.activeSelf)
            {
                inventoryUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                sterowanie.active = true;
                playerRigid.constraints = RigidbodyConstraints.FreezeRotation;
                playerState = PlayerState.Playing;
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
                playerState = PlayerState.InInvetory;
            }
        }

        // Open/Close Pause Menu Or Close CodeUI
        if(Input.GetKeyDown(KeyCode.Escape) && time <= 0 && (playerState == PlayerState.Playing || playerState == PlayerState.Paused || playerState == PlayerState.Decoding))
        {
            if (codeUI.activeSelf)
            {
                codeUI.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                sterowanie.active = true;
                playerRigid.constraints = RigidbodyConstraints.FreezeRotation;

                backupButton = null;
                playerState = PlayerState.Playing;
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
                    playerState = PlayerState.Playing;
                }
                else
                {
                    pauseMenuUI.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    sterowanie.active = false;
                    time = betweenInputs;
                    playerRigid.constraints = RigidbodyConstraints.FreezeAll;
                    playerState = PlayerState.Paused;
                }
            }
        }

        // Changing Selected Slot
        if (playerState == PlayerState.Playing)
        {
            if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
            {
                selectedSlot = 0;
                RefreshSelectedSlot();
            }

            if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
            {
                selectedSlot = 1;
                RefreshSelectedSlot();
            }

            if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
            {
                selectedSlot = 2;
                RefreshSelectedSlot();
            }

            if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4))
            {
                selectedSlot = 3;
                RefreshSelectedSlot();
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                int toAdd = (int)Input.mouseScrollDelta.y % 4;
                if (selectedSlot + toAdd > 3)
                {
                    toAdd = toAdd - 4;
                }
                if (selectedSlot + toAdd < 0)
                {
                    toAdd = toAdd + 4;
                }
                selectedSlot += toAdd;
                RefreshSelectedSlot();
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
                    playerState = PlayerState.Decoding;


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
                playerState = PlayerState.Decoding;


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

            for (int i = 0; i < choosedItemsID.Length; i++)
            {
                if(choosedItemsID[i] == -1)
                {
                    SelectItem(i,baseItem.GetID(), baseItem.GetImage());
                    break;
                }
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
                    SelectItem(selectedSlot, -1);
                }
                break;
            }
        }
    }

    public void SelectItem(int slotID, int id, Sprite image = null)
    {
        if(slotID != -1)
        {
            if (id == -1)
            {
                slotsIMG[slotID].sprite = null;
                choosedItemsID[slotID] = -1;
            }
            else
            {
                slotsIMG[slotID].sprite = image;
                choosedItemsID[slotID] = id;
            }
            RefreshSelectedSlot();
        }
    }

    /// <summary>
    /// Spawns selected item in hand
    /// </summary>
    /// <param name="id">this is id of item in Item Database</param>
    void GiveItemInHand(int id)
    {
        if (objectInHand != null)
        {
            Destroy(objectInHand);
            objectInHand = null;
        }

        if (id != -1)
        {
            choosedItemID = id;
            objectInHand = Instantiate(itemDatabase.itemPrefabs[choosedItemID], handTransform);
            if (objectInHand.GetComponent<Rigidbody>() != null)
            {
                objectInHand.GetComponent<Rigidbody>().useGravity = false;
                objectInHand.GetComponent<Rigidbody>().isKinematic = true;
                objectInHand.GetComponent<Rigidbody>().freezeRotation = true;
            }
            if (objectInHand.GetComponent<Collider>() != null)
            {
                objectInHand.GetComponent<Collider>().enabled = false;
            }
            objectInHand.transform.localScale = itemDatabase.itemInHandSize[choosedItemID];
        }
    }

    /// <summary>
    /// Helps to get item from Item Database
    /// </summary>
    /// <param name="id">id of item in Item Database</param>
    /// <returns>Item object from Item Database</returns>
    public Item GetItem(int id)
    {
        return itemDatabase.itemPrefabs[id].GetComponent<Item>();
    }

    public void RefreshSelectedSlot()
    {
        for(int i = 0; i<slotsBgIMG.Length; i++)
        {
            if (slotsBgIMG[i] == slotsBgIMG[selectedSlot])
            {
                slotsBgIMG[i].color = selectedSlotColor;
                GiveItemInHand(choosedItemsID[selectedSlot]);
            }
            else
            {
                slotsBgIMG[i].color = notSelectedSlotColor;
            }
        }
    }
}
