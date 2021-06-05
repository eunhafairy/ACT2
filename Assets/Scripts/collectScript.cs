using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class collectScript : MonoBehaviour
{
    GameObject audioManager;
    Animator animator;
    
    int score;
    bool flag = false;
    [SerializeField]private int tunaw;
    GameObject collectibles, player;
    private void Start()
    {
        tunaw = 0;
        this.GetComponent<Light2D>().enabled = false;
        audioManager = GameObject.Find("MusicManager");
        player = GameObject.Find("Cat");
        collectibles = GameObject.Find("Collectibles");
        animator = GetComponent<Animator>();
        animator.SetBool("isSpecial", false);
    }
    private void Update()
    {
        animator.SetInteger("tunaw", tunaw);
        if (this.tag == "specialBomb") {
            this.GetComponent<Light2D>().enabled = true;
            animator.SetBool("isSpecial", true);
            if (!flag) {
                StartCoroutine(tunawin());
            }
        }
        if (collectibles.transform.childCount < 1) {
            StopAllCoroutines();
            player.layer = 11;
        }
    }
    IEnumerator tunawin() {
        flag = true;
        yield return new WaitForSeconds(5);
        tunaw = 1;
        yield return new WaitForSeconds(5);
        tunaw = 2;
        yield return new WaitForSeconds(5);
        tunaw = 3;
        yield return new WaitForSeconds(5);
        tunaw = 4;
      
      
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" || collision.tag == "invulnerablePlayer") {

            gameObject.GetComponent<Light2D>().enabled = false;
            if (this.gameObject.tag == "specialBomb")
            {

             
                audioManager.transform.GetChild(3).gameObject.GetComponent<AudioSource>().Play();
                score = PlayerPrefs.GetInt("Score");
                score += 20;
                PlayerPrefs.SetInt("Score", score);
                animator.SetTrigger("score20");
           
            }
            else {


               
                audioManager.transform.GetChild(4).gameObject.GetComponent<AudioSource>().Play();
                score = PlayerPrefs.GetInt("Score");
                score += 10;
                PlayerPrefs.SetInt("Score", score);
                animator.SetTrigger("score10");

            }
           
            transform.parent = null;
            StartCoroutine(selfDestruct());
         
            
        }
    }

    IEnumerator selfDestruct() {
        GetComponent<CircleCollider2D>().enabled = false;
      
        if (collectibles.transform.childCount < 1)
        {
            Destroy(this.gameObject);
            yield break;
        }
        else {
            
         
            yield return new WaitForSeconds(2);
            Destroy(this.gameObject);
        }
        
    }

    
}
