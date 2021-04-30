using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class InputFieldChecker : MonoBehaviour
{
    public InputField inputField;
    public UnityEvent stuffToDoTrue;
    public UnityEvent stuffToDoFalse;

    private void OnEnable()
    {
        inputField.text = "";
        updateState();
    }

    public void updateState()
    {
        if(inputField.text.Length > 0 && inputField.text != "" && CountSpaces(inputField.text) != inputField.text.Length)
        {
            if(stuffToDoTrue != null)
            {
                stuffToDoTrue.Invoke();
            }
        }
        else
        {
            if (stuffToDoFalse != null)
            {
                stuffToDoFalse.Invoke();
            }
        }
    }

    int CountSpaces(string text)
    {
        int count = -1;
        if (text.Length > 0)
        {
            count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if(text.ToCharArray()[i] == ' ')
                {
                    count++;
                }
            }
        }
        return count;
    }
}
