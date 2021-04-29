using MAIPA.Interactable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeHandler : MonoBehaviour
{
    public PlayerScript player;
    public GameObject textCodeUI;
    public GameObject numCodeUI;
    [HideInInspector]
    public CodeType codeType;

    [Header("For text Code:")]
    string text;
    public InputField textField;

    [Header("For num Code:")]
    int num1;
    int num2;
    int num3;
    int num4;
    public InputField num1Field;
    public InputField num2Field;
    public InputField num3Field;
    public InputField num4Field;

    public void UpdateUI()
    {
        textCodeUI.SetActive(codeType == CodeType.TEXT_CODE ? true : false);
        numCodeUI.SetActive(codeType == CodeType.NUM_CODE ? true : false);
    }

    public void SetNumCode(int num1, int num2, int num3, int num4)
    {
        this.num1 = num1;
        this.num2 = num2;
        this.num3 = num3;
        this.num4 = num4;
    }

    public void SetTextCode(string text)
    {
        this.text = text;
    }

    public void CheckTextCode()
    {
        if(textField.text == text)
        {
            player.ButtonInteract();
        }
    }

    public void ChcekNumCode()
    {
        if(num1.ToString() == num1Field.text && num2.ToString() == num2Field.text && num3.ToString() == num3Field.text && num4.ToString() == num4Field.text)
        {
            player.ButtonInteract();
        }
    }
}
