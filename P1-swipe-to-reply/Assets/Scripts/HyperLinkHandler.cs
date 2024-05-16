using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HyperLinkHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Ŭ���� ��ũ�� �ִ� ��ġ ��������
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(GetComponent<TextMeshProUGUI>(), Input.mousePosition, null);

        if (linkIndex != -1)
        {
            // Ŭ���� ��ũ�� ���� ��������
            TMP_TextInfo textInfo = GetComponent<TextMeshProUGUI>().textInfo;
            TMP_LinkInfo linkInfo = textInfo.linkInfo[linkIndex];

            // �ش� ��ũ�� �̵�
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
