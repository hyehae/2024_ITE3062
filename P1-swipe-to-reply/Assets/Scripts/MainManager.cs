using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public static int ScreenWidth = Screen.width;
    public static int ScreenHeight = Screen.height;
    
    public StringBuilder sb = new StringBuilder();
    public StringBuilder resultSb = new StringBuilder();
    public int previousSceneNumber = -1;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if(ScreenWidth == Screen.width || ScreenHeight == Screen.height)
        {
            ScreenHeight = Screen.width - (640 * 2);
            ScreenWidth = Screen.height - (360 * 2);
            Screen.SetResolution(ScreenWidth, ScreenHeight, true);
        }
    }

}
