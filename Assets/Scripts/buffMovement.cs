using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class buffMovement : MonoBehaviour
{
    [SerializeField] float speed;
    Vector2 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        speed = 2f;
        moveDirection = new Vector2(1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        moveBall();
        if (moveDirection.x == 1)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
        }
        else {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
    void moveBall() {
        transform.Translate(moveDirection * speed * Time.deltaTime);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "wall") {

            if (moveDirection.x == 1)
            {
               
              
                moveDirection.x = -1;
            } else {
                
                moveDirection.x = 1;
            }

        }
        if (collision.gameObject.tag == "roof" || collision.gameObject.tag == "platform") {
         
            if (moveDirection.y == 1)
            {
               
                moveDirection.y = -1;
            }
            else
            {
        
                moveDirection.y = 1;
            }
        }
    }

}
