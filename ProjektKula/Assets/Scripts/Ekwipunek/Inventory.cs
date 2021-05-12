using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public PlayerScript player;
    public QuickMenuUI quickMenu;
    public GameObject itemPrefab;
    public Transform inventory;
    [HideInInspector]
    public List<ItemUI> itemUIs = new List<ItemUI>();

    public void Refresh()
    {
        foreach (var item in itemUIs)
        {
            Destroy(item.gameObject);
        }
        itemUIs = new List<ItemUI>();

        foreach (var item in player.items)
        {
            ItemUI itemUI = Instantiate(itemPrefab, inventory).GetComponent<ItemUI>();
            itemUI.itemImage.sprite = item.GetImage();
            itemUI.itemNumText.text = "x" + item.GetNum();
            itemUI.itemID = item.GetID();
            for(int i = 0; i < player.choosedItemsID.Length; i++)
            {
                if (item.GetID() == player.choosedItemsID[i])
                {
                    itemUI.selectedText.SetActive(true);
                    quickMenu.itemsUI[i] = itemUI;
                    quickMenu.itemsIDs[i] = item.GetID();
                    break;
                }
                else
                {
                    itemUI.selectedText.SetActive(false);
                }
            }
            itemUI.quickMenuUI = quickMenu;
            itemUIs.Add(itemUI);
        }

        quickMenu.selectedID = player.selectedSlot;
        quickMenu.RefreshSelected();
        quickMenu.RefreshSlots();
    }
}
