using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMechanics : MonoBehaviour {

	Rigidbody2D c_rb;

	float f_h;
	//float f_v;
	public float f_speed;
	public float f_jumpforce;
	
	public bool b_stunned;
	public bool b_grounded;
	public bool b_falling;

	public bool b_canstuntop;

	public RaycastHit2D r_hitTop;
	public RaycastHit2D r_hitDown;
	
	public float f_stunDelay;
	
	// Use this for initialization
	void Start () {
		c_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update() {
		if(GameManager.instance.i_lives > 0 && !b_stunned && b_grounded)
		{
			if(Input.GetKeyDown(KeyCode.UpArrow))
			{
				Jump();
			}
		}
	}
	void FixedUpdate () {
		Debug.DrawLine(gameObject.transform.position,new Vector2(gameObject.transform.position.x,gameObject.transform.position.y + 0.33f),Color.blue);
		Debug.DrawLine(gameObject.transform.position,new Vector2(gameObject.transform.position.x,gameObject.transform.position.y - 0.32f),Color.blue);
		if(GameManager.instance.i_lives > 0)
		{
			if(!GetComponent<BoxCollider2D>().isTrigger)
			{
				isGrounded();
			}
			if(!b_stunned)
			{
				if(b_grounded)
				{
					Move();
				}
			}
			
			if(b_falling && b_grounded)
			{
				b_falling = false;
				StopCoroutine(Stun());
				StartCoroutine(Stun());
			}
			if(b_canstuntop && !GetComponent<BoxCollider2D>().isTrigger)
			{
				StunTop();
			}
		}
	}


	public void Move()
	{
		f_h = Input.GetAxis("Horizontal");
		c_rb.velocity = new Vector2(f_h * f_speed,c_rb.velocity.y);
	}

	public void Jump()
	{
		b_canstuntop = true;
		c_rb.velocity = new Vector2(0,f_jumpforce);
		SlowMotionOn();
	}

	public void Fall()
	{
		
		c_rb.velocity = new Vector2(0,c_rb.velocity.y);
		
		SlowMotionOn();
	}

	public void isGrounded()
	{
		r_hitDown = Physics2D.Raycast(gameObject.transform.position,Vector2.down);
		if(r_hitDown.collider != null)
		{
			if(r_hitDown.distance <= 0.35f && r_hitDown.collider.gameObject.tag == "Ground")
			{
				b_grounded = true;
				SlowMotionOff();
			}else
			{
				b_grounded = false;
				Fall();
			}
		}
	}

	
	public void StunTop()
	{
		r_hitTop = Physics2D.Raycast(gameObject.transform.position,Vector2.up);
		if(r_hitTop.collider != null)
		{
			if(r_hitTop.distance <= 0.33f && r_hitTop.collider.gameObject.tag == "Ground")
			{
				StopCoroutine(Stun());
				StartCoroutine(Stun());
			}
		}
		
	}

	IEnumerator Stun()
	{
		b_stunned = true;
		b_canstuntop = false;
		c_rb.velocity = new Vector2(0,c_rb.velocity.y);
		//animacion de stun

		yield return new WaitForSeconds(f_stunDelay);
		b_stunned = false;
		
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Wall")
		{
			gameObject.transform.position = new Vector3(col.gameObject.transform.position.x,gameObject.transform.position.y,gameObject.transform.position.z);
		}

		if(col.gameObject.tag == "Hole")
		{
			
			if(b_grounded)
			{
				Fall();
				b_grounded = false;
				b_falling = true;
			}else if(!b_grounded && !gameObject.GetComponent<BoxCollider2D>().isTrigger && !b_stunned && b_canstuntop)
			{
				GameManager.instance.b_canSpanwHole = true;
			}
			gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
		}

		if(col.gameObject.tag == "Enemy")
		{
			StartCoroutine(Stun());
		}
	}

	public void OnTriggerExit2D(Collider2D col)
	{
		if(col.gameObject.tag == "Hole")
		{
			gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
			b_canstuntop = false;
		}
	}

	public void SlowMotionOn()
	{
		Time.timeScale = 0.65f;
	}

	public void SlowMotionOff()
	{
		Time.timeScale = 1;
	}
}
