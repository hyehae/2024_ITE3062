using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class SendEmail : MonoBehaviour
{
    // Ŭ���̾�Ʈ���� ������ ��û�� ������ �Լ�
    public void SendEmailRequest(EmailData emailData)
    {
        // ������ URL
        string serverURL = "http://127.0.0.1:5500/tempServer.html";

        // ������ ������ JSON ������
        string requestData = JsonUtility.ToJson(emailData);

        // ������ POST ��û ������
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

    // ����: �̸��� ������ ��û
    public void ExampleRequest(StringBuilder sb)
    {
        EmailData emailData = new EmailData();
        emailData.to = "email@naver.com";
        emailData.subject = "Hello from Client";
        emailData.body = sb.ToString();

        // Ŭ���̾�Ʈ���� ������ �̸��� ������ ��û ������
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
