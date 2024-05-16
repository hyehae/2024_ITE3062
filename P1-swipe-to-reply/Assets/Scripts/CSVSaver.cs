using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class CSVSaver : MonoBehaviour
{
    public string fileName = "Setting.csv";

    public InputField nameText;
    public InputField countryText;
    public InputField yearText;
    public InputField heightText;

    List<string[]> data = new List<string[]>();
    string[] tempData;

    void Awake()
    {
        data.Clear();

        tempData = new string[4];
        tempData[0] = "Name";
        tempData[1] = "Country";
        tempData[2] = "Year";
        tempData[3] = "Height";
        data.Add(tempData);
    }

    public void SaveCSVFile()
    {
        tempData = new string[4];
        tempData[0] = nameText.text;
        tempData[1] = countryText.text;
        tempData[2] = yearText.text;
        tempData[3] = heightText.text;
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

        // string filepath = SystemPath.GetPath();
        string filepath = getPath();

        if (!Directory.Exists(filepath))
        {
            Directory.CreateDirectory(filepath);
        }

        StreamWriter outStream = System.IO.File.CreateText(filepath + fileName);
        outStream.Write(sb);
        outStream.Close();
    }

    private string getPath()
    {
        #if UNITY_EDITOR   
            return Application.dataPath +"/CSV/“+”/Student Data.csv";        
        #elif UNITY_ANDROID        
            return Application.persistentDataPath+"Student Data.csv";        
        #elif UNITY_IPHONE        
            return Application.persistentDataPath+"/"+"Student Data.csv";        
        #else        
            return Application.dataPath +"/"+"Student Data.csv";        
        #endif    
    }

    private void OnApplicationQuit()
    {
        SaveCSVFile();
    }
}