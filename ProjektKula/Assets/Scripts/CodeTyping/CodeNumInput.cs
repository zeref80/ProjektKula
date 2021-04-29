using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CodeNumInput : MonoBehaviour
{
    int num;
    public InputField numField;

    private void OnEnable()
    {
        num = 0;
        numField.text = num.ToString();
    }

    public void Increment()
    {
        num = num + 1 > 9 ? 0 : num + 1;
        numField.text = num.ToString();
    }

    public void Decrement()
    {
        num = num - 1 < 0 ? 9 : num - 1;
        numField.text = num.ToString();
    }
}
