using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitApplication : MonoBehaviour
{
    void Start()
    {
        // 버튼 클릭 이벤트에 Quit 함수를 연결합니다.
        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(Quit);
        }
        else
        {
            Debug.LogError("Button component not found!");
        }
    }

    void Quit()
    {
#if UNITY_EDITOR
        // Unity 에디터에서 실행 중이면 플레이 모드를 중지합니다.
        EditorApplication.isPlaying = false;
#else
        // Unity 에디터가 아닌 경우에는 애플리케이션을 종료합니다.
        Application.Quit();
#endif
    }
}
