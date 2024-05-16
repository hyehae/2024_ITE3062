using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TouchScreenKeyboardInputField : MonoBehaviour
{
    //private TouchScreenKeyboard keyboard;
    public TMP_InputField inputField;

    void Start()
    {
        WebGLInput.captureAllKeyboardInput = false;

        if (inputField != null)
        {
            inputField.ActivateInputField();
        }
    }
    
    /*void Start()
    {
        ShowKeyboard();
    }

    void Update()
    {
        if (keyboard != null && !keyboard.active)
        {
            ShowKeyboard();
        }
    }

    void ShowKeyboard()
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
    
    
        if (inputField != null)
        {
            inputField.ActivateInputField();
        }
    }*/
}
