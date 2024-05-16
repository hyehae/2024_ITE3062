using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class SendEmail : MonoBehaviour
{
    // 클라이언트에서 서버로 요청을 보내는 함수
    public void SendEmailRequest(EmailData emailData)
    {
        // 서버의 URL
        string serverURL = "http://127.0.0.1:5500/tempServer.html";

        // 서버에 전송할 JSON 데이터
        string requestData = JsonUtility.ToJson(emailData);

        // 서버에 POST 요청 보내기
        StartCoroutine(SendRequest(serverURL, requestData));
        //StartCoroutine(SendRequest(serverURL, sb.ToString()));
    }

    IEnumerator SendRequest(string url, string data)
    {
        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Network response was not ok: " + www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Email sent successfully: " + responseText);
            }
        }
    }

    // 예시: 이메일 보내기 요청
    public void ExampleRequest(StringBuilder sb)
    {
        EmailData emailData = new EmailData();
        emailData.to = "email@naver.com";
        emailData.subject = "Hello from Client";
        emailData.body = sb.ToString();

        // 클라이언트에서 서버로 이메일 보내기 요청 보내기
        SendEmailRequest(emailData);
    }
}

[System.Serializable]
public class EmailData
{
    public string to;
    public string subject;
    public string body;
}
