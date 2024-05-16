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
        yield return new WaitForSeconds(0.5f); // 0.5�� ���� ����մϴ�.

        // ������ Scene���� ��ȯ�մϴ�.
        BubbleSwipeAction.writeSb = false;
        MainManager.Instance.previousSceneNumber = currentIndex;
        SceneManager.LoadScene(currentIndex+1);
    }
}
