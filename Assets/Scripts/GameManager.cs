using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject title;
    public GameObject splash;
    public GameObject ScoreSystem;
    public Text scoreText;
    public Text BestScoreText;
    public int pointsWorth = 1;
    
    private GameObject player;
    private spawner spawner;
    private Vector2 screenBounds;
    private int score;
    private int BestScore = 0;
    private bool gameStarted = false;
    private bool smokeCleared = true;
    private bool BeatBestScore;

    void Awake()
    {
        spawner = GameObject.Find("Spawner").GetComponent<spawner>();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        player = playerPrefab;
        BestScoreText.enabled = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        spawner.active = false;
        title.SetActive(true);
        splash.SetActive(false);

        BestScore = PlayerPrefs.GetInt("BestScore");
        BestScoreText.text = "Best Score:" + BestScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        //IF THE GAME IS NOT RUNNING...
        if (!gameStarted)
        {
            var textColor = "#FF0";

            if (BeatBestScore)
            {
                textColor = "#F00";
            }
            BestScoreText.text = "<color=" + textColor + ">Best Score:" + BestScore.ToString() + "</color>";

            if (Input.anyKeyDown && smokeCleared)
            {
                smokeCleared = false;
                ResetGame();
            }
            else
            {
                BestScoreText.text = "";
            }

            var nextBomb = GameObject.FindGameObjectsWithTag("Bomb");

            foreach (GameObject bombObject in nextBomb)
            {
                Destroy(bombObject);
            }
        }

        //IF THE GAME IS RUNNING...
        else {
            if (!player)
            {
                OnPlayerKilled();
            }

            var nextBomb = GameObject.FindGameObjectsWithTag("Bomb");

            foreach (GameObject bombObject in nextBomb)
            {
                if (!gameStarted)
                {
                    Destroy(bombObject);
                }
                else if (bombObject.transform.position.y < -screenBounds.y)
                {
                    ScoreSystem.GetComponent<Score>().AddScore(pointsWorth);
                    Destroy(bombObject);
                }
            }
        }
    }       

    void ResetGame()
    {
        spawner.active = true;
        title.SetActive(false);
        splash.SetActive(false);
        

        scoreText.enabled = true;
        ScoreSystem.GetComponent<Score>().score = 0;
        ScoreSystem.GetComponent<Score>().Start();

        BeatBestScore = false;
        BestScoreText.enabled = true;
        player = Instantiate(playerPrefab, new Vector3(0, 0, 0), playerPrefab.transform.rotation);
        gameStarted = true;
    }

    void OnPlayerKilled()
    {
        spawner.active = false;
        gameStarted = false;

        Invoke("SplashScreen", 2f);

        score = ScoreSystem.GetComponent<Score>().score;

        if (score > BestScore)
        {
            BestScore = score;
            PlayerPrefs.SetInt("BestScore", BestScore);
            BeatBestScore = true;
            BestScoreText.text = "Best Score:" + BestScore.ToString();
        }
    }
    void SplashScreen()
    {
        smokeCleared = true;
        splash.SetActive(true);
    }
    
}
