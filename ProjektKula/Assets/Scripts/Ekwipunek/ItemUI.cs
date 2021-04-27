using MAIPA.Interactable;
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

    [Header("Info Object:")]
    public GameObject infoObject;
    GameObject infoOnScene;

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

    public void SpawnInfo()
    {
        PlayerScript player = FindObjectOfType<PlayerScript>();
        infoOnScene = Instantiate(infoObject, player.inventoryUI.transform);
        float xOffset = infoOnScene.GetComponent<RectTransform>().rect.width / 2 + GetComponent<RectTransform>().rect.width / 2 + 15;
        float yOffset = infoOnScene.GetComponent<RectTransform>().rect.height / 2 - GetComponent<RectTransform>().rect.height / 2;
        float xPos = transform.position.x + xOffset * ((float)Screen.width / 1920f);
        float yPos = transform.position.y - yOffset * ((float)Screen.height / 1080f);
        if (xPos + (infoOnScene.GetComponent<RectTransform>().rect.width / 2) * ((float)Screen.width / 1920f) >= Screen.width)
        {
            xPos = transform.position.x - xOffset * ((float)Screen.width / 1920f);
        }

        if (yPos - (infoOnScene.GetComponent<RectTransform>().rect.height / 2) * ((float)Screen.height / 1080f) <= 0)
        {
            yPos = transform.position.y + yOffset * ((float)Screen.height / 1080f);
        }

        Vector2 pos = new Vector2(xPos, yPos);
        infoOnScene.transform.position = pos;
        Item item = player.GetItem(itemID);
        infoOnScene.GetComponent<ItemInfoUI>().itemName.text = item.itemName;
        infoOnScene.GetComponent<ItemInfoUI>().itemInfo.text = item.itemInfo;
    }

    public void DeleteInfo()
    {
        Destroy(infoOnScene);
    }
}
