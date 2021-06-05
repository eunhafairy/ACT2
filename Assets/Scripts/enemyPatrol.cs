using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;
public class enemyPatrol : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float distance;
    bool movingRight = true;
    [SerializeField] Transform groundDetect, wallDetect;

    private void Awake()
    {
        StartCoroutine(placeTag());
    }
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetect.position, Vector2.down, distance);
       
        if (groundInfo.collider == false) {
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                movingRight = false;
            }
            else {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
     
        if (collision.gameObject.tag == "wall") {
           
            if (movingRight)
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                movingRight = true;
            }
        }
    }

    IEnumerator placeTag()
    {
        this.gameObject.layer = 9;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f,1f,1f,0.5f);
        yield return new WaitForSeconds(2);
        gameObject.GetComponent<Light2D>().enabled = true;
        this.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        this.gameObject.layer = 7;
        if (this.gameObject.tag == "Untagged")
        {
            
            this.gameObject.tag = "addedEnemy";
        }
    }
}
