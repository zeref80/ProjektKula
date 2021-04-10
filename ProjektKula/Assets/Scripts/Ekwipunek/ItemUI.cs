using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    public int itemID;
    public Image itemImage;
    public Text itemNumText;
    public GameObject selectedText;

    public void SelectItem()
    {
        if (selectedText.activeSelf)
        {
            FindObjectOfType<PlayerScript>().SelectItem(-1);
            selectedText.SetActive(false);
        }
        else
        {
            FindObjectOfType<PlayerScript>().SelectItem(itemID, itemImage.sprite);
            selectedText.SetActive(true);
        }
    }
}
