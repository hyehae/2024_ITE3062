using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SwipeDetector : MonoBehaviour
{
    // private Image bubbleImage;
    public CSVFileWriter targetCSVFileWriter; // = GetComponent<CSVFileWriter>();

    private Vector2 fingerDownPos;
    private Vector2 fingerUpPos;

    public bool detectSwipeAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;

    // Update is called once per frame
    void Update()
    {

        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                fingerUpPos = touch.position;
                fingerDownPos = touch.position;
            }

            //Detects Swipe while finger is still moving on screen
            if (touch.phase == TouchPhase.Moved)
            {
                // initial touch가 UI image라면 손가락 따라 position transform하기

                if (!detectSwipeAfterRelease)
                {
                    fingerDownPos = touch.position;
                    DetectSwipe();
                }
            }

            //Detects swipe after finger is released from screen
            if (touch.phase == TouchPhase.Ended)
            {
                // UI image 원래 위치로 되돌리기
                // isUIImageSelected = false;

                fingerDownPos = touch.position;
                DetectSwipe();
            }
        }
    }

    void DetectSwipe()
    {

        if (VerticalMoveValue() > SWIPE_THRESHOLD && VerticalMoveValue() > HorizontalMoveValue())
        {
            //Debug.Log("Vertical Swipe Detected!");
            if (fingerDownPos.y - fingerUpPos.y > 0)
            {
                OnSwipeUp();
            }
            else if (fingerDownPos.y - fingerUpPos.y < 0)
            {
                OnSwipeDown();
            }
            fingerUpPos = fingerDownPos;

        }

        // 수평 방향으로 변하는 Touch의 position이 수직 방향보다 큰 경우
        else if (HorizontalMoveValue() > SWIPE_THRESHOLD && HorizontalMoveValue() > VerticalMoveValue())
        {
            // 터치의 x좌표가 커지면 왼->오 swipe
            if (fingerDownPos.x - fingerUpPos.x > 0)
            {
                targetCSVFileWriter.SaveCSVFile(HorizontalMoveValue(), VerticalMoveValue(), "Left to Right");
                OnSwipeRight(HorizontalMoveValue());
            }

            // 터치의 x좌표가 작아지면 오->왼 swipe
            else if (fingerDownPos.x - fingerUpPos.x < 0)
            {
                targetCSVFileWriter.SaveCSVFile(VerticalMoveValue(), HorizontalMoveValue(), "Right to Left");
                OnSwipeLeft(HorizontalMoveValue()*(-1));
            }
            fingerUpPos = fingerDownPos;

        }
        else
        {
            Debug.Log("No Swipe Detected!");
        }            //Debug.Log("Horizontal Swipe Detected!");

    }

    float VerticalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.y - fingerUpPos.y);
    }

    float HorizontalMoveValue()
    {
        return Mathf.Abs(fingerDownPos.x - fingerUpPos.x);
    }

    void OnSwipeUp()
    {
        //Do something when swiped up
        Debug.Log("Swipe Up!");
    }

    void OnSwipeDown()
    {
        //Do something when swiped down
        Debug.Log("Swipe Down!");
    }

    void OnSwipeLeft(float moveValue)
    {
        //Do something when swiped left
        BubbleSwipeAction.BubbleSwipeLeft(moveValue);
    }

    void OnSwipeRight(float moveValue)
    {
        //Do something when swiped right
        BubbleSwipeAction.BubbleSwipeRight(moveValue);
    }
}