using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class OnScreenKeyboardInputfield : MonoBehaviour //, IPointerDownHandler
{
    public OnScreenKeyboard targetOnScreenKeyboard;
    private InputField _inputField;
    public string inputtedString;

    public Button sendButton;
    private Text blueBubbleText;
    private static Image WritingReplyMsg;

    private void Awake()
    {
        _inputField = GetComponent<InputField>();
        
        if (_inputField == null)
            return;

        // _inputField.shouldHideMobileInput = true;
        if (targetOnScreenKeyboard)
        {
            targetOnScreenKeyboard.gameObject.SetActive(true);
            targetOnScreenKeyboard.ShowKeyboard(_inputField, this);
        }
    }


    public void SaveInputedString(string _inputStr)
    {
        inputtedString = _inputStr;
    }

    /*public void OnPointerDown(PointerEventData eventData)
    {
        if (targetOnScreenKeyboard)
        {
            targetOnScreenKeyboard.gameObject.SetActive(true);
            targetOnScreenKeyboard.ShowKeyboard(_inputField, this);
        }
    }*/


    // sendButton이 onClick이면
    // 1) blueBubble의 text에 inputtedString 넣기
    // 2) inputtedString 초기화
    // 3) blueBubble setActive
    public void SendMsg()
    {
        blueBubbleText = GameObject.FindGameObjectWithTag("BlueBubbleText").GetComponent<Text>();
        WritingReplyMsg = GameObject.Find("WritingReplyMsg").GetComponent<Image>();

        WritingReplyMsg.enabled = false;

        blueBubbleText.text = inputtedString;
        _inputField.text = "";
    }

}
