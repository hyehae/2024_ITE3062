using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SetBubbleText : MonoBehaviour
{
    public Text Text;

    StringBuilder resultSb;

    void Awake()
    {
        if (MainManager.Instance != null)
        {
            resultSb = MainManager.Instance.resultSb;
            Text.text = resultSb.ToString();
        }
        else
        {
            resultSb = new StringBuilder();
        }

    }
    
}
