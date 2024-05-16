using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuitApplication : MonoBehaviour
{
    void Start()
    {
        // ��ư Ŭ�� �̺�Ʈ�� Quit �Լ��� �����մϴ�.
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
        // Unity �����Ϳ��� ���� ���̸� �÷��� ��带 �����մϴ�.
        EditorApplication.isPlaying = false;
#else
        // Unity �����Ͱ� �ƴ� ��쿡�� ���ø����̼��� �����մϴ�.
        Application.Quit();
#endif
    }
}
