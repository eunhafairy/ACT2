using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class ComputeScore : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] TextMeshProUGUI scoreTMP, liveTMP, totalTMP;
    [SerializeField] GameObject gameOver1, gameOver2, buttons, audioManager;
    bool flag;
    [SerializeField]int score, life, total;
    [SerializeField] Animator transition;
    void Start()
    {
        transition.SetTrigger("End");
        audioManager = GameObject.Find("AudioManager");

        buttons.SetActive(false);
        flag = false;
        gameOver1.SetActive(false);
        gameOver2.SetActive(false);
        if (PlayerPrefs.GetInt("isGameOver") == 1)
        {
            gameOver1.SetActive(true);
            StartCoroutine(scoreAdd());
        }
        else {
            gameOver2.SetActive(true);
        }
        
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            buttons.SetActive(true);
            audioManager.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Stop();
            audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Stop();
            life = PlayerPrefs.GetInt("Life");
            score = PlayerPrefs.GetInt("Score");
            total = (score * life);
        }
    }

    private void FixedUpdate()
    {
      
        scoreTMP.SetText(score.ToString());
        liveTMP.SetText(life.ToString());
        totalTMP.SetText(total.ToString());
    }
    IEnumerator scoreAdd() {

        while (!flag) {
            score = 0;
            life = 0;
            total = 0;
            yield return new WaitForSeconds(2);
            while (life != PlayerPrefs.GetInt("Life"))
            {
                life++;
                yield return new WaitForSeconds(0.01f);

            }

            yield return new WaitForSeconds(0.5f);
            audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
            while (score != PlayerPrefs.GetInt("Score"))
            {
                score++;
                yield return new WaitForSeconds(0.001f);

            }
            audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Stop();
            yield return new WaitForSeconds(0.5f);
            audioManager.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
            while (total != (score * life))
            {
                total++;
                yield return new WaitForSeconds(0.0001f);
            }
            audioManager.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Stop();
            yield return new WaitForSeconds(0.5f);
            buttons.SetActive(true);
            flag = true;
        }
        
        

    }

    public void restartGame() {
        transition.SetTrigger("Start");
        Invoke("restart", 2);
    }
    public void quitGame() {
        transition.SetTrigger("Start");
        Invoke("quit",2);
    }

    void restart() {
        SceneManager.LoadScene(1);
    }

    void quit()
    {
        Application.Quit();
    }


}
