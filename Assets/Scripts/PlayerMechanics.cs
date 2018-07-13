using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMechanics : MonoBehaviour {

	Rigidbody2D c_rb;

	float f_h;
	//float f_v;
	public float f_speed;
	public float f_jumpforce;
	
	public bool b_alive;
	public bool b_stunned;
	public bool b_grounded;

	
	public RaycastHit2D r_hit;
	
	public int i_lives;
	// Use this for initialization
	void Start () {
		c_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(i_lives>0)
		{
			b_alive = true;
		}else
		{
			b_alive = false;
		}
		if(b_alive)
		{
			r_hit = Physics2D.Raycast(gameObject.transform.position,Vector2.down);
			if(r_hit.collider != null)
			{
				if(r_hit.distance <= 0.5f && r_hit.collider.gameObject.tag == "Ground")
				{
					b_grounded = true;
				}else
				{
					b_grounded = false;
					c_rb.velocity = new Vector2(0,c_rb.velocity.y);
				}
			}

			if(!b_stunned)
			{
				if(b_grounded)
				{
					f_h = Input.GetAxis("Horizontal");
					c_rb.velocity = new Vector2(f_h * f_speed,c_rb.velocity.y);
					if(Input.GetKeyDown(KeyCode.UpArrow))
					{
						c_rb.velocity = new Vector2(0,f_jumpforce);
					}
				}
			}
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Wall")
		{
			gameObject.transform.position = new Vector2(col.gameObject.transform.position.x,gameObject.transform.position.y);
		}
	}

	public void SlowMotionOn()
	{
		Time.timeScale = 0.3f;
	}

	public void SlowMotionOff()
	{
		Time.timeScale = 1;
	}
}
