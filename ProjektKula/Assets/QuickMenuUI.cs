using JetBrains.Annotations;
using MAIPA.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickMenuUI : MonoBehaviour
{
    public PlayerScript player;
    public int selectedID = 0;
    public Image[] images;
    public Image[] bgImages;
    public Color selectedColor;
    public Color notSelectedColor;

    public int[] itemsIDs = { -1, -1, -1, -1 };
    public ItemUI[] itemsUI = { null, null, null, null };

    private void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1) || Input.GetKey(KeyCode.Keypad1))
        {
            selectedID = 0;
            RefreshSelected();
        }

        if (Input.GetKey(KeyCode.Alpha2) || Input.GetKey(KeyCode.Keypad2))
        {
            selectedID = 1;
            RefreshSelected();
        }

        if (Input.GetKey(KeyCode.Alpha3) || Input.GetKey(KeyCode.Keypad3))
        {
            selectedID = 2;
            RefreshSelected();
        }

        if (Input.GetKey(KeyCode.Alpha4) || Input.GetKey(KeyCode.Keypad4))
        {
            selectedID = 3;
            RefreshSelected();
        }

        if(Input.mouseScrollDelta.y != 0)
        {
            int toAdd = (int)Input.mouseScrollDelta.y % 4;
            if(selectedID + toAdd > 3)
            {
                toAdd = toAdd - 4;
            }
            if(selectedID + toAdd < 0)
            {
                toAdd = toAdd + 4;
            }
            selectedID += toAdd;
            RefreshSelected();
        }
    }

    // Refresh Funcs
    public void RefreshSelected()
    {
        foreach (var bg in bgImages)
        {
            if (bg == bgImages[selectedID])
            {
                bg.color = selectedColor;
            }
            else
            {
                bg.color = notSelectedColor;
            }
        }
    }

    public void RefreshSlots()
    {
        for (int i = 0; i < itemsIDs.Length; i++)
        {
            if (itemsIDs[i] != -1)
            {
                bool founded = false;
                foreach (var itemUI in player.inventory.itemUIs)
                {
                    if (itemUI.itemID == itemsIDs[i])
                    {
                        SelectItem(i, itemsIDs[i], player.GetItem(itemsIDs[i]).itemImage, itemUI);
                        founded = true;
                    }
                }
                if (!founded)
                {
                    itemsUI[i] = null;
                    images[i].sprite = null;
                    itemsIDs[i] = -1;
                }
            }
        }
    }

    // Managing Items
    public void SelectItem(int itemId, Sprite itemImage, ItemUI itemUI)
    {
        DeselectItemBySlot(selectedID);
        DeselectItemByID(itemId);
        images[selectedID].sprite = itemImage;
        itemsIDs[selectedID] = itemId;
        itemsUI[selectedID] = itemUI;
        itemUI.selectedText.SetActive(true);
        player.SelectItem(selectedID, itemId, itemImage);
    }

    public void SelectItem(int itemSlot, int itemId, Sprite itemImage, ItemUI itemUI)
    {
        DeselectItemBySlot(itemSlot);
        DeselectItemByID(itemId);
        images[itemSlot].sprite = itemImage;
        itemsIDs[itemSlot] = itemId;
        itemsUI[itemSlot] = itemUI;
        itemUI.selectedText.SetActive(true);
        player.SelectItem(itemSlot, itemId, itemImage);
    }

    public void DeselectItemByID(int itemId)
    {
        for(int i = 0; i < itemsIDs.Length; i++)
        {
            if (itemsIDs[i] == itemId)
            {
                itemsUI[i] = null;
                images[i].sprite = null;
                itemsIDs[i] = -1;
                player.SelectItem(i, -1);
                break;
            }
        }
    }

    public void DeselectItemBySlot(int itemSlot)
    {
        if (itemsIDs[itemSlot] != -1)
        {
            itemsUI[itemSlot].selectedText.SetActive(false);
            itemsUI[itemSlot] = null;
            images[itemSlot].sprite = null;
            itemsIDs[itemSlot] = -1;
            player.SelectItem(itemSlot, -1);
        }
    }
}
