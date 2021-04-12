using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHandler
{
    int itemID;
    string itemName;
    Sprite itemImage;
    string itemInfo;
    int numberOfItems;

    public ItemHandler(int id, string name, Sprite image, string info, int num = 1)
    {
        itemID = id;
        itemName = name;
        itemImage = image;
        itemInfo = info;
        numberOfItems = num;
    }

    public int GetID()
    {
        return itemID;
    }

    public string GetName()
    {
        return itemName;
    }

    public Sprite GetImage()
    {
        return itemImage;
    }

    public string GetInfo()
    {
        return itemInfo;
    }

    public int GetNum()
    {
        return numberOfItems;
    }

    public string Show()
    {
        return numberOfItems + "x Item NO." + itemID + "\nName: " + itemName + "\nInfo: " + itemInfo;
    }

    public void AddItem(int numToAdd)
    {
        numberOfItems += numToAdd;
    }
}
