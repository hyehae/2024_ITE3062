using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CSVFileWriter : MonoBehaviour
{
    public SendEmail targetSendEmail;

    public string fileName = "TouchPosition.csv";
    private int previousSceneIndex;
    private int currentSceneIndex;

    List<string[]> data = new List<string[]>();
    string[] tempData;
    StringBuilder finalSb;
    StringBuilder resultSb;

    void Awake()
    { 
        if(MainManager.Instance != null)
        {
            finalSb = MainManager.Instance.sb;
            resultSb = MainManager.Instance.resultSb;
            previousSceneIndex = MainManager.Instance.previousSceneNumber;
        } else
        {
            finalSb = new StringBuilder();
            resultSb = new StringBuilder();
            previousSceneIndex = currentSceneIndex;
        }

        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 3)
        {
            Debug.Log(resultSb.ToString());
        }

        data.Clear();

        tempData = new string[4];
        tempData[0] = "HorizontalMoveValue";
        tempData[1] = "VerticalMoveValue";
        tempData[2] = "Direction";
        tempData[3] = "SceneNumber";
        data.Add(tempData);
    }

    public void SaveCSVFile(float horizontalMoveValue, float verticalMoveValue, string direction)
    {
        //StoreResult(currentSceneIndex, direction);

        //MainManager.Instance.previousSceneNumber = currentSceneIndex;
        tempData = new string[4];
        tempData[0] = horizontalMoveValue.ToString("F3");
        tempData[1] = verticalMoveValue.ToString("F3");
        tempData[2] = direction;
        tempData[3] = currentSceneIndex.ToString();
        data.Add(tempData);

        string[][] output = new string[data.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = data[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < length; i++)
        {
            sb.AppendLine(string.Join(delimiter, output[i]));
        }

        finalSb.AppendLine(sb.ToString());
        MainManager.Instance.sb = finalSb;

        // string filepath = SystemPath.GetPath();
        string filepath = getPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);

            StreamWriter outStream = System.IO.File.CreateText(filepath + fileName);
            //targetSendEmail.ExampleRequest(sb);
            //finalSb.AppendLine(sb.ToString());
            outStream.Write(sb);
            outStream.Close();
        }

        else
        {
            StreamWriter outStream = File.AppendText(filepath + fileName);
            //targetSendEmail.ExampleRequest(sb);
            //finalSb.AppendLine(sb.ToString());
            outStream.Write(sb);
            outStream.Close();
        }
        
    }
    
    private string getPath()
    {
#if UNITY_EDITOR
            return Application.dataPath +"/CSV/TouchPosition.csv";        
#elif UNITY_ANDROID
        return Path.Combine(Application.persistentDataPath, "TouchPosition.csv");        
#elif UNITY_IPHONE
        return Path.Combine(Application.persistentDataPath, "TouchPosition.csv");    
#else
        return Application.dataPath + "/" + "TouchPosition.csv";
#endif
    }
}