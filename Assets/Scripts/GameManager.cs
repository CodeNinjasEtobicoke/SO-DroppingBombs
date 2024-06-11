using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private spawner spawner;
    public GameObject title;
    private Vector2 screenBounds;
    public GameObject playerPrefab;
    private GameObject player;
    private bool gameStarted = false;
    public GameObject splash;
    public GameObject ScoreSystem;
    public Text scoreText;
    public int pointsWorth = 1;
    private int score;
    private bool smokeCleared = true;
    private int BestScore = 0;
    public Text BestScoreText;
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
        if (!gameStarted)
        {
            var textColor = "#FFF2FC";
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
                if (!player)
                {
                    OnPlayerKilled();
                }

            }


            var nextBomb = GameObject.FindGameObjectsWithTag("Bomb");

            foreach (GameObject bombObject in nextBomb)
            {
                if (!gameStarted)
                {
                    Destroy(bombObject);
                }
                else if (bombObject.transform.position.y < (-screenBounds.y))
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
            player = Instantiate(playerPrefab, new Vector3(0, 0, 0), playerPrefab.transform.rotation);
            gameStarted = true;

            scoreText.enabled = true;
            ScoreSystem.GetComponent<Score>().score = 0;
            ScoreSystem.GetComponent<Score>().Start();

            BeatBestScore = false;
            BestScoreText.enabled = true;
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
