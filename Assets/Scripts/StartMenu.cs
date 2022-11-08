using System.Collections;
using System.Collections.Generic;
using System.Security.Authentication.ExtendedProtection.Configuration;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class StartMenu : MonoBehaviour
{
    private string nameInputted;


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
        nameInputted = s;
        Debug.Log(nameInputted);
        Debug.Log(Application.persistentDataPath);
        NameSave();
    }

    public class SaveData
    {
        public string name;
        public int highScore;
    }

    public void NameSave()
    {
        //creating new object of the class that will hold the data. This allows different saves
        SaveData data = new SaveData();

        //setting the data inside the newly created object, equal to the desired value
        data.name = nameInputted;

        //creating a string to hold the value of the entire class & write it to a JSON file
        string json = JsonUtility.ToJson(data);

        //writing the string variable "json" to a JSON file.
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }


}
