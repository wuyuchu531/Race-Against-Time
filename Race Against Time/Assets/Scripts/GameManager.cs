using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



//this namespace will only be included when we're compiling within Unity Editor
#if UNITY_EDITOR
using UnityEditor;
#endif


public class GameManager : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public GameObject powerupPrefab;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI pastScoreText;
    public GameObject titleScreen;
    public GameObject pausePanel;
    public GameObject scorePanel;
    public GameObject gameOverScreen;
    private bool paused;
    public bool isGameActive;
    //public AudioSource gmAudio;
    
    
    
    private int score;
    public int pastScore;
    private const string ScoreKey = "PlayerScore";
    private int timeCount = 60;
    private float spawnRangeX = 16;
    private float interval = 3;
    private float spawnPosZ = -4;

    private float powerStartTime = 1;
    private float powerSpawnTime = 4;
    private float enemyStartTime = 2;
    private float enemySpawnTime = 3;



    // Start is called before the first frame update
    void Start()
    {
        int loadedScore = LoadScore();
        pastScoreText.text = "Past Score: " + loadedScore;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeToPause();
        }
    }

    
    //spawn enemies
    void SpawnRandomEnemy()
    {
        if(isGameActive == true)
        {
            int enemyIndex = Random.Range(0, enemyPrefab.Length);
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
            Instantiate(enemyPrefab[enemyIndex], spawnPos, enemyPrefab[enemyIndex].transform.rotation);
        } 
    }

    //spawn power
    void SpawnPower()
    {
        if (isGameActive == true)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX + interval, spawnRangeX - interval), 0, spawnPosZ);
            Instantiate(powerupPrefab, spawnPos, powerupPrefab.transform.rotation);
        }     
    }

    //start the game
    public void StartGame()
    {
        titleScreen.gameObject.SetActive(false);
        isGameActive = true;
        score = 0;
        UpdateScore(0);
        StartCoroutine(Timer());
        //gmAudio.Play();

        //spawn enemy and power
        InvokeRepeating("SpawnRandomEnemy", enemyStartTime, enemySpawnTime); //call this method at 2 seconds, then repeat every 3 seconds
        InvokeRepeating("SpawnPower", powerStartTime, powerSpawnTime);  //call this method at 1 second, then repeat every 2 seconds

    }

    //set the timer
    IEnumerator Timer()
    {
        while(isGameActive && timeCount > 0)
        {
            timerText.text = "Time: " + timeCount;
            yield return new WaitForSeconds(1);
            timeCount--;
        }

        if(timeCount == 0)
        {
            GameOver();
        }
    }


    //update the score
    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    //save the score
    public void OnDestroy()
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }

    //load the saved score
    public int LoadScore()
    {
       return PlayerPrefs.GetInt(ScoreKey, score);
    }


    //show score record
    public void ShowScoreRecord()
    {
        titleScreen.gameObject.SetActive(false);
        scorePanel.gameObject.SetActive(true);
        int loadedScore = LoadScore();
        pastScoreText.text = "Past Score: " + loadedScore;
    }

    //close score panel
    public void CloseScorePanel()
    {
        titleScreen.gameObject.SetActive(true);
        scorePanel.gameObject.SetActive(false);
    }


    //game over
    public void GameOver()
    {
        isGameActive = false;
        gameOverScreen.gameObject.SetActive(true);
    }

    //restart game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //pause game
    void ChangeToPause()
    {
        if (!paused)
        {
            paused = true;
            pausePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            paused = false;
            pausePanel.gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    //change volume
    public void GoToVolumeMenu()
    {
        SceneManager.LoadScene(1);
    }


    //exit game
    public void ExitGame()
    {
        OnDestroy();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

