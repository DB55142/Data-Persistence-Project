using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string name;

    public Text UserName;

    public Text highScore;
    private int prevHighScore;
    private string prevHighScoreUser;
    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }

        GetName();
        DisplayName();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            ExitGame();
        }

        DisplayUserName();
        DisplayHighScore();
        GetHighScore();
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GetName()
    {
        //plugging desired file path
        string path = Application.persistentDataPath + "/savefile.json";

        //if the file exists
        if (File.Exists(path))
        {
            //reading in all text from the specified JSON file
            string json = File.ReadAllText(path);

            //creating an object of the "SaveData" class from the StartMenu script to hold the class data from the JSON file
            StartMenu.SaveData data = JsonUtility.FromJson<StartMenu.SaveData>(json);

            //setting value of the variable from the JSON file as the value to a local variable
            name = data.name;
        }
    }

    public void GetHighScore()
    {
        string path = Application.persistentDataPath + "/highscore.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            HighScoreSaveData hSData = JsonUtility.FromJson<HighScoreSaveData>(json);

            prevHighScore = hSData.points;
            prevHighScoreUser = hSData.name;
        }

        else
        {
            Debug.Log("file isn't there");
        }
    }

    private void DisplayUserName()
    {
        UserName.text = "User: " + name;
    }

    private void DisplayHighScore()
    {
        highScore.text = "High Score from " + prevHighScoreUser + ": " + prevHighScore;
    }

    public void DisplayName()
    {
        Debug.Log(name);
    }
    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        UpdateHighScore();
    }

    public class HighScoreSaveData
    {
        public string name;
        public int points;
    }


    public void UpdateHighScore()
    {
        if (m_Points > prevHighScore)
        {
            HighScoreSaveData hSData = new HighScoreSaveData();

            hSData.name = name;
            hSData.points = m_Points;

            string json = JsonUtility.ToJson(hSData);

            File.WriteAllText(Application.persistentDataPath + "/highscore.json", json);
            Debug.Log(hSData.points);
        }
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();

#else
        Application.Quit();

#endif
    }
}
