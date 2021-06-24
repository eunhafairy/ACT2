using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;
using Pathfinding;
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] GameObject gameOverScreen, livesLeftScreen, defaultEnemyObject, audioManager, pauseBuff;
    [SerializeField] Animator transition;
    AudioSource bgMusic;
    public CharacterControllerScript characterController;
    public Animator animator;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false, glide = false, slide = false, canClick = true, isDead = false, isPaused = false, pauseFlag;
    int life;

    
    GameObject pauseSpawn, defaultEnemy, GameManager;
    List<GameObject> toDestroyEnemyList = new List<GameObject>();

    private void Start()
    {
   
        
        GameManager = GameObject.Find("GameManager");
        bgMusic = audioManager.transform.GetChild(0).gameObject.GetComponent<AudioSource>();
        InvokeRepeating("canClickAgain", 0, 1);
    }
    void Update()
    {


      

        if (GameManager.GetComponent<GameManager>().timeLeft <= 0) {

            isDead = true;
         
            characterController.enabled = false;
            this.gameObject.tag = "playerTemp";
            runSpeed = 0;
            AudioFade.FadeOut(GetComponent<AudioSource>(), 1);
            animator.SetBool("isDead", true);
            this.gameObject.layer = 11;
            transition.SetTrigger("Start");
            PlayerPrefs.SetInt("isGameOver", 2);
            Invoke("goToGameOverScreen", 3);
        }
    
        this.transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        life = PlayerPrefs.GetInt("Life");
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (!isDead) {
            if (Input.GetButtonDown("Jump"))
            {


                jump = true;


            }
            if (Input.GetMouseButtonDown(0))
            {
                glide = false;
                characterController.dashMove();


            }
            if (GetComponent<Rigidbody2D>().velocity.y < 0) {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    glide = true;

                }
            }
           
            else
            {
                glide = false;

            }
            if (Input.GetMouseButtonDown(1))
            {
                if (canClick)
                {
                    animator.SetBool("isSliding", true);
                    slide = true;
                    canClick = false;
                }




            }

        }
    

    }

    void canClickAgain() {
        canClick = true;
    
    }
    private void FixedUpdate()
    {
       
        characterController.Move(horizontalMove * Time.fixedDeltaTime, jump, glide, slide);
        jump = false;
        slide = false;
    }
    public void OnLanding() {

        glide = false;
        characterController.isDashing = false;
        animator.SetBool("isDashing", false);
        characterController.playerGravity = 1f;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

    }
    void goToGameOverScreen()
    {
        SceneManager.LoadScene(2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform") {

         
            characterController.m_Grounded = true;
        }
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "addedEnemy")
        {
            if (this.gameObject.layer == 1) {
                return; 
                }
            else if (this.gameObject.tag == "Player") {
                if (life <= 1)
                {
                    isDead = true;
                    characterController.enabled = false;
                    this.gameObject.tag = "playerTemp";
                    runSpeed = 0;
                    this.gameObject.transform.LookAt(collision.gameObject.transform);

                    if (gameObject.GetComponent<SpriteRenderer>().flipX)
                    {
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000, 0));
                    }
                    else
                    {
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2000, 0));
                    }

                    AudioFade.FadeOut(GetComponent<AudioSource>(), 1);
                    animator.SetBool("isDead", true);
                    this.gameObject.layer = 11;
                   
                    transition.SetTrigger("Start");
                    PlayerPrefs.SetInt("isGameOver", 2);
                  
                    Invoke("goToGameOverScreen",3);
                }
                else
                {
                    PlayerPrefs.SetInt("Time", GameManager.GetComponent<GameManager>().timeLeft);
                    GameManager.GetComponent<GameManager>().CancelInvoke("Timer");
                    isDead = true;
                    animator.SetBool("isDead", true);
                    bgMusic.Stop();
                    audioManager.transform.GetChild(5).gameObject.GetComponent<AudioSource>().Play();
                    transition.SetTrigger("Start");
                   
                    //disable player and change tag so it doesn't register twice
                    this.gameObject.tag = "playerTemp";
                    this.gameObject.layer = 11;
                    this.gameObject.transform.LookAt(collision.gameObject.transform);

                    if (gameObject.GetComponent<SpriteRenderer>().flipX)
                    {
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(2000, 0));
                    }
                    else {
                        this.gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(-2000, 0));
                    }
                    
                    characterController.enabled = false;
                    runSpeed = 0;
                
                    life = PlayerPrefs.GetInt("Life");
                    life -= 1;
                    PlayerPrefs.SetInt("Life", life);
                    StartCoroutine(enemyTouch());

                }
            }
           


        }
        if (collision.gameObject.tag == "buff") {

            Animator buffDeath = collision.gameObject.GetComponent<Animator>();
            buffDeath.SetTrigger("death");
            collision.gameObject.GetComponent<Light2D>().enabled = false;
            audioManager.transform.GetChild(1).gameObject.GetComponent<AudioSource>().Play();
            //pause the spawnenemy script
            pauseSpawn = GameObject.FindGameObjectWithTag("GameManager");
            pauseSpawn.GetComponent<GameManager>().CancelInvoke("SpawnEnemy");
            pauseSpawn.GetComponent<GameManager>().CancelInvoke("SpawnBuff");
            StartCoroutine(DestroyBuff(collision.gameObject));
            StartCoroutine(freezeEnemy());

        }

        if (collision.gameObject.tag == "freezeEnemy" || collision.gameObject.tag == "freezeAddedEnemy")
        {
          int score = PlayerPrefs.GetInt("Score")+ 10;
            audioManager.transform.GetChild(6).gameObject.GetComponent<AudioSource>().Play();
          PlayerPrefs.SetInt("Score", score);
            if (collision.gameObject.tag == "freezeEnemy")
            {
                collision.gameObject.layer = 9;
                collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1f);
                collision.gameObject.GetComponent<Animator>().SetTrigger("patDeath");
                collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;

            }
            else {
                collision.gameObject.layer = 9;
                collision.gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
                collision.gameObject.GetComponent<Animator>().SetTrigger("death");
                collision.gameObject.GetComponent<AIDestinationSetter>().enabled = false;
                collision.gameObject.GetComponent<enemyGFX>().enabled = false;
                collision.gameObject.GetComponent<AIPath>().enabled = false;
                collision.gameObject.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                collision.gameObject.GetComponent<Rigidbody2D>().gravityScale =1;
            }
            
         // collision.gameObject.SetActive(false);
          toDestroyEnemyList.Add(collision.gameObject);


        }
    }
    IEnumerator DestroyBuff(GameObject buff) {
        buff.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        buff.GetComponent<buffMovement>().enabled = false;
        buff.GetComponent<CircleCollider2D>().enabled = false;
        yield return new WaitForSeconds(1);
        Destroy(buff);
    }
    IEnumerator enemyTouch()
    {
        
        yield return new WaitForSeconds(3);

        livesLeftScreen.SetActive(true);
        animator.SetBool("isDead", false);
        
        yield return new WaitForSeconds(3);

        //set timer
        GameManager.GetComponent<GameManager>().timeLeft = PlayerPrefs.GetInt("Time");
        GameManager.GetComponent<GameManager>().InvokeRepeating("Timer",1,1);
        GameManager.GetComponent<GameManager>().changeCourt = !GameManager.GetComponent<GameManager>().changeCourt;
       
        isDead = false;
        DestroyAddedEnemies("addedEnemy");
        GameObject buff = GameObject.FindGameObjectWithTag("buff");
        Destroy(buff);
        DestroyAddedEnemies("Enemy");
        livesLeftScreen.SetActive(false);
        //place player
        bgMusic.Play();
        runSpeed = 30f;
       
        defaultEnemy = GameObject.FindGameObjectWithTag("defaultEnemy");
        Destroy(defaultEnemy);
        Instantiate(defaultEnemyObject);
        transition.SetTrigger("End");
        this.gameObject.tag = "Player";
        this.gameObject.layer = 6;
        characterController.enabled = true;
        this.transform.position = new Vector2(0.25f, -4f);
        this.transform.rotation = Quaternion.identity;
       

    }
    IEnumerator freezeEnemy() {
        //get all gameObjects with enemy layer
        
        GameObject[] gameObjectArray = FindObjectsOfType<GameObject>();
        List<GameObject> enemyList = new List<GameObject>();

        for (int i = 0; i < gameObjectArray.Length; i++) {
            if (gameObjectArray[i].layer == 7 || gameObjectArray[i].layer == 9) {
                enemyList.Add(gameObjectArray[i]);
            }

        }
        if (enemyList.Count == 0) {
          
            yield return 0;
        }
        else {
           
            foreach (var temp in enemyList)
            {
                if (temp.tag == "Enemy")
                {
                    temp.tag = "freezeEnemy";
                    temp.GetComponent<enemyPatrol>().enabled = false;
                }
                else {
                    temp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
                    temp.tag = "freezeAddedEnemy";
                }
                temp.GetComponent<Animator>().SetBool("isCoin", true);
            }

        }

        yield return new WaitForSeconds(4);
        
        //blink lights
        foreach (var temp in enemyList)
        {
            if (temp.layer == 7) {
                temp.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.5f);
            }
            

        }
        yield return new WaitForSeconds(0.3f);
        foreach (var temp in enemyList)
        {
            if (temp.layer == 7)
            temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);

        }
        yield return new WaitForSeconds(0.3f);
        foreach (var temp in enemyList)
        {
            if (temp.layer == 7)
                temp.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.5f);

        }
        yield return new WaitForSeconds(0.3f);
        foreach (var temp in enemyList)
        {
            if (temp.layer == 7)
                temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1,1);

        }
        yield return new WaitForSeconds(0.3f);
        foreach (var temp in enemyList)
        {
            if (temp.layer == 7)
                temp.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0, 0.5f);

        }
        yield return new WaitForSeconds(0.3f);
       


        pauseSpawn.GetComponent<GameManager>().InvokeRepeating("SpawnEnemy", 0, 10);
        pauseSpawn.GetComponent<GameManager>().InvokeRepeating("SpawnBuff", 10, 20);
        //unfreeze
        foreach (var temp in enemyList)
        {
            

            if (temp.tag == "freezeEnemy")
            {
                temp.GetComponent<enemyPatrol>().enabled = true;
                temp.tag = "Enemy";
            }
            else {
                temp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
                temp.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
                temp.tag = "addedEnemy";
            }
            temp.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            temp.GetComponent<Animator>().SetBool("isCoin", false);

        }

        //destroy tagged enemies
        foreach (var temp in toDestroyEnemyList) {
            Destroy(temp);
            }
       



    }

    void DestroyAddedEnemies(string tag) {
        GameObject[] toDestroyEnemy = GameObject.FindGameObjectsWithTag(tag);
        for (int i = 0; i<toDestroyEnemy.Length; i++) {
            Destroy(toDestroyEnemy[i]);
        }
    
    }


}
