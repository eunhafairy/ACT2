using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuScript : MonoBehaviour
{
    public Animator animator;
    public GameObject credits, menu;
    private void Start()
    {
    
        credits.SetActive(false);
     
        menu.SetActive(true);
    }
    public void startGame() {
        StartCoroutine(LoadLevel());
    }
    public IEnumerator LoadLevel() {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(1);

    }
    public void exitGame() {
        Application.Quit();
    }
    public void showCredits() {
        menu.SetActive(false);
        credits.SetActive(true);

    }
    public void hideCredits()
    {
        menu.SetActive(true);
        credits.SetActive(false);

    }


}
