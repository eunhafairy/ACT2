using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class activeCollectible : MonoBehaviour
{
    [SerializeField] GameObject audioManager;
    AudioSource bgMusic;
    public GameObject gameOverScreen, player;
    public TextMeshProUGUI scoreTMP;
    public Animator transition;
    private void Start()
    {
        bgMusic = audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        player = GameObject.Find("Cat");
        gameOverScreen.SetActive(false);
    }
    void Update()
    {
        if (transform.childCount > 0) //taga check if meron pang child (collectibles), if zero na, nakuha na lahat. game over na
        {
            
            Transform specialBomb = this.transform.GetChild(0); //para makuha ung first child. sya ung magiging active
            specialBomb.tag = "specialBomb"; //nilagyan ng tag na sya ung active
            specialBomb.GetComponent<SpriteRenderer>().color = new Color(255, 71, 71, 1); //binago ung kulay ng active
        }
        else {
            StartCoroutine(AudioFade.FadeOut(bgMusic, 2f));
            player.layer = 11;
            transition.SetTrigger("Start");
            PlayerPrefs.SetInt("isGameOver",1);
            Invoke("goToGameOverScreen", 1);
            
           
        }
        
        
    }
    void goToGameOverScreen() {
        SceneManager.LoadScene(2);
    }
    
}
