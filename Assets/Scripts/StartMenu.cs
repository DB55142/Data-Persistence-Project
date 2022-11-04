using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    private string name;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#else
        Application.Quit();

#endif
    }

    public void Name(string s)
    {
        name = s;
        Debug.Log(name);
    }
}
