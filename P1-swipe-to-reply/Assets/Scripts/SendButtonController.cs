using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SendButtonController : MonoBehaviour
{
    public Button sendButton;
    private int currentIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = SceneManager.GetActiveScene().buildIndex;
        sendButton.onClick.AddListener(StartSceneTransition);
    }

    private void StartSceneTransition()
    {
        StartCoroutine(SceneTransitionCoroutine());
    }

    private IEnumerator SceneTransitionCoroutine()
    {
        yield return new WaitForSeconds(0.5f); // 0.5초 동안 대기합니다.

        // 지정된 Scene으로 전환합니다.
        BubbleSwipeAction.writeSb = false;
        MainManager.Instance.previousSceneNumber = currentIndex;
        SceneManager.LoadScene(currentIndex+1);
    }
}
