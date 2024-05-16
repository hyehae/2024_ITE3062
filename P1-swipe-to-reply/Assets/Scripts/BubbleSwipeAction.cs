using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Text;
using UnityEngine.SceneManagement;


public class BubbleSwipeAction : MonoBehaviour, IDragHandler, IEndDragHandler
{

    private RectTransform rectTransform;
    private Vector3 originalPosition;
    private Vector3 modifiedPosition;
    private static float moveX;

    private static Image WritingReplyMsg;

    public RectTransform objectRectTransform;

    private static string direction;
    public static bool writeSb = false;

    StringBuilder resultSb;
    private int currentSceneIndex;
    private int previousSceneIndex;
    

    void Awake()
    {
        if (MainManager.Instance != null)
        {
            resultSb = MainManager.Instance.resultSb;
            previousSceneIndex = MainManager.Instance.previousSceneNumber;
        }
        else
        {
            resultSb = new StringBuilder();
            previousSceneIndex = currentSceneIndex;
        }

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

    }


    public void OnDrag(PointerEventData eventData)
    {
        // 드래그 이벤트가 발생한 위치를 가져옵니다.
        Vector2 dragPosition = eventData.position;

        // 드래그 이벤트가 발생한 위치를 RectTransform의 부모 객체를 기준으로 한 상대적인 위치로 변환합니다.
        Vector2 localDragPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            objectRectTransform.parent as RectTransform,
            dragPosition,
            eventData.pressEventCamera,
            out localDragPosition
        );

        //rectTransform.position = new Vector3 (eventData.position.x, originalPosition.y, originalPosition.z);

        //rectTransform.position = new Vector3(originalPosition.x + moveX, originalPosition.y, originalPosition.z);
        //modifiedPosition.x += moveX;
        rectTransform.anchoredPosition = new Vector3(localDragPosition.x, originalPosition.y, originalPosition.z);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition = originalPosition;
        WritingReplyMsg.enabled = true;

        if (writeSb == false)
        {
            resultSb.AppendLine("Scene " + (currentSceneIndex + 1).ToString() + ": " + direction);
            MainManager.Instance.resultSb = resultSb;
            writeSb = true;
        }
    }

    void Start()
    {
        WritingReplyMsg = GameObject.Find("WritingReplyMsg").GetComponent<Image>();

        rectTransform = GetComponent<RectTransform>();
        //originalPosition = rectTransform.position;
        originalPosition = rectTransform.anchoredPosition;
        modifiedPosition = originalPosition;
        Debug.Log(originalPosition.x + " " + originalPosition.y + " " + originalPosition.z);
    }

    public static void BubbleSwipeRight(float moveValue)
    {
        Debug.Log("Swipe Right!");
        direction = "Left to Right";
        moveX = moveValue;
        //rectTransform.position = new Vector3(originalPosition.x + moveValue, originalPosition.y, originalPosition.z);
        // WritingReplyMsg.enabled = true;
    }

    public static void BubbleSwipeLeft(float moveValue)
    {
        Debug.Log("Swipe Left!");
        direction = "Right to Left";
        moveX = moveValue;
        //rectTransform.position = new Vector3(originalPosition.x + moveValue, originalPosition.y, originalPosition.z);

        // WritingReplyMsg.enabled = true;
    }
}
