using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class HyperLinkHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭한 링크가 있는 위치 가져오기
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(GetComponent<TextMeshProUGUI>(), Input.mousePosition, null);

        if (linkIndex != -1)
        {
            // 클릭한 링크의 정보 가져오기
            TMP_TextInfo textInfo = GetComponent<TextMeshProUGUI>().textInfo;
            TMP_LinkInfo linkInfo = textInfo.linkInfo[linkIndex];

            // 해당 링크로 이동
            Application.OpenURL(linkInfo.GetLinkID());
        }
    }
}
