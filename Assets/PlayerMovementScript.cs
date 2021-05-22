using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    public CharacterControllerScript characterController;
    public Animator animator;
    public float runSpeed = 40f;
    float horizontalMove = 0f;
    bool jump = false;
    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump")) {

         
                jump = true;
                animator.SetBool("isJumping", true);

        }
        if (Input.GetMouseButtonDown(0)) {

                characterController.dashMove();
        
         
        }
    }
    private void FixedUpdate()
    {
        characterController.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
    public void OnLanding() {
        Debug.Log("Landed");
        animator.SetBool("isJumping", false);
        animator.SetBool("isDashing", false);
        transform.GetComponent<Rigidbody2D>().gravityScale = 1f;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

    }
}
