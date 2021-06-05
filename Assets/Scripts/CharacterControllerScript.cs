using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class CharacterControllerScript : MonoBehaviour
{
	public Animator animator;
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;                         // Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] public float playerGravity = 1f;
	public float k_GroundedRadius = .4f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	[SerializeField] float m_glideSpeed = 0.3f, glideGravity = 4;
	private Rigidbody2D m_Rigidbody2D;
	
	private Vector3 m_Velocity = Vector3.zero;
	public bool isGliding= false, isDashing = false, isSliding = false;
	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	
	

	private void Awake()
	{
		animator = GetComponent<Animator>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		
	}

	private void Update()
	{

		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				//if (wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	
		if (!isGliding && !isDashing) {
			m_Rigidbody2D.gravityScale = playerGravity;
		}
		if (m_Grounded) {
			isGliding = false;
		}
	
		animator.SetBool("isJumping", !m_Grounded);
		animator.SetBool("isGliding", isGliding);



	}

	public void dashMove() {
		
        if (!m_Grounded) {
			isGliding = false;
			animator.SetBool("isDashing", true);
		
			StartCoroutine(dash());
			

		}
	}


	public void Move(float move, bool jump, bool glide, bool slide)
	{

		if (slide) {
			StartCoroutine(slideMove(slide, move));
		}
		
		isGliding = glide;
		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{

			// If crouching
			if (glide && !m_Grounded)
			{

				move *= m_glideSpeed;
				m_Rigidbody2D.gravityScale = playerGravity/glideGravity;


			}



			Debug.Log(move);
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			
			if (move > 0 )
			{
				GetComponent<SpriteRenderer>().flipX = false;
			}
			
			else if (move < 0 )
			{

				GetComponent<SpriteRenderer>().flipX = true;
			}
		}
		// If the player should jump...
		if (m_Grounded && jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			animator.SetBool("isJumping", true);
			
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	IEnumerator slideMove(bool slide, float move) {
		if (slide && m_Grounded)
		{
			if (GetComponent<SpriteRenderer>().flipX)
			{
				m_Rigidbody2D.AddForce(new Vector2(-2000 , 0));

			}
			else {
				m_Rigidbody2D.AddForce(new Vector2(2000 , 0));
			}
			
			isSliding = true;
			
		}
		yield return new WaitForSeconds(0.3f);
		animator.SetBool("isSliding", false);
		isSliding = false;
			}
	IEnumerator dash() {

		isDashing = true;
		m_Rigidbody2D.velocity = Vector3.zero;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
		yield return new WaitForSeconds(0.5f);
		playerGravity = 20f;
		m_Rigidbody2D.gravityScale = playerGravity;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;


	}
}
