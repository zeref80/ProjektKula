using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public PlayerScript player;
    public GameObject itemPrefab;
    public Transform inventory;
    [HideInInspector]
    public List<ItemUI> itemUIs;

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
            if (item.GetID() == player.choosedItemID)
            {
                itemUI.selectedText.SetActive(true);
            }
            else
            {
                itemUI.selectedText.SetActive(false);
            }
            itemUIs.Add(itemUI);
        }
    }
}
