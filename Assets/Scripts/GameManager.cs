using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI scoreTMP, gameOverScore, timerTMP;
    int score, life;
    public bool showTutorial;
    [SerializeField] GameObject livesLeftPanel, enemy, fly_enemy, audioManager;
    public GameObject buffs, tutorial;
    float halfScreenSizeX;
    public int timeLeft;
    [SerializeField]Transform lives, lives2;
    AudioSource bgMusic;

    // Start is called before the first frame update
    void Start()
    {
        
        showTutorial = true;
        tutorial = GameObject.Find("Tutorial");
        tutorial.SetActive(showTutorial);
        
        bgMusic = audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        bgMusic.Play();
        PlayerPrefs.SetInt("Time", 60);
        timeLeft = PlayerPrefs.GetInt("Time");
        PlayerPrefs.SetInt("isGameOver", 2);


        halfScreenSizeX = Camera.main.aspect * Camera.main.orthographicSize;
        InvokeRepeating("Timer", 1,1);
        InvokeRepeating("SpawnBuff", 15, 20);
        InvokeRepeating("SpawnEnemy", 10, 20);
        Time.timeScale = 0f;
        livesLeftPanel.SetActive(false);
        PlayerPrefs.SetInt("Score", 0);
        PlayerPrefs.SetInt("Life", 3);
    }

    public void Timer() {
        timeLeft--;
        PlayerPrefs.SetInt("Time",timeLeft);
    
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Time.timeScale = 1f;
            showTutorial = false;
        }
        tutorial.SetActive(showTutorial);
        timerTMP.SetText("Time Left: "+ timeLeft);
        score = PlayerPrefs.GetInt("Score");
        life = PlayerPrefs.GetInt("Life");
        scoreTMP.SetText(score.ToString());
        if (life == 3) {
            lives.GetChild(0).gameObject.SetActive(true);
            lives.GetChild(1).gameObject.SetActive(true);
            lives.GetChild(2).gameObject.SetActive(true);

            lives2.GetChild(0).gameObject.SetActive(true);
            lives2.GetChild(1).gameObject.SetActive(true);
            lives2.GetChild(2).gameObject.SetActive(true);
        }
        else if (life == 2)
        {

            lives.GetChild(0).gameObject.SetActive(true);
            lives.GetChild(1).gameObject.SetActive(true);
            lives.GetChild(2).gameObject.SetActive(false);

            lives2.GetChild(0).gameObject.SetActive(true);
            lives2.GetChild(1).gameObject.SetActive(true);
            lives2.GetChild(2).gameObject.SetActive(false);

        }
        else if (life == 1) {
            lives.GetChild(0).gameObject.SetActive(true);
            lives.GetChild(1).gameObject.SetActive(false);
            lives.GetChild(2).gameObject.SetActive(false);

            lives2.GetChild(0).gameObject.SetActive(true);
            lives2.GetChild(1).gameObject.SetActive(false);
            lives2.GetChild(2).gameObject.SetActive(false);

        }
        else if (life == 0)
        {
            lives.GetChild(0).gameObject.SetActive(false);
            lives.GetChild(1).gameObject.SetActive(false);
            lives.GetChild(2).gameObject.SetActive(false);

        }
      
        gameOverScore.SetText(score.ToString());
    }
  
  
    void SpawnBuff() {
        GameObject buffExist = GameObject.FindGameObjectWithTag("buff");

        if (buffExist == null) {
            audioManager.transform.GetChild(2).gameObject.GetComponent<AudioSource>().Play();
            Vector2 randomPos = new Vector2(Random.Range(-halfScreenSizeX + 7, halfScreenSizeX - 7), Random.Range(-4, 4));
            Instantiate(buffs, randomPos, Quaternion.identity);
        }
        
      
     
    }
    void SpawnEnemy() {
        int chance = Random.Range(1, 10);
        Vector2 randomPos = new Vector2(Random.Range(-halfScreenSizeX + 7, halfScreenSizeX - 7), Random.Range(-4, 4));
        if (chance <= 5)
        {

            Instantiate(enemy, randomPos, Quaternion.identity);
        }
        else
        {
            Instantiate(fly_enemy, randomPos, Quaternion.identity);
        }
    }
}

