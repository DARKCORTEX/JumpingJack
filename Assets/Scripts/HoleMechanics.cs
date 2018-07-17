using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleMechanics : MonoBehaviour {

	Rigidbody2D c_rb;
	public float f_speed;
	public GameObject g_holeClone;
	public int i_groundLvl;
	float f_limitX = 4.813f;
	float f_posX = -5.803f;

	public bool b_activated;
	public enum MoveDirection
	{
		Left,
		Right
	};

	public MoveDirection direction;
	// Use this for initialization
	void Start () {
		c_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Move();
		Teleport();
	}

	public void Move()
	{
		if(GameManager.instance.b_canMoveHole)
		{
			c_rb.velocity = new Vector2((direction == MoveDirection.Right?f_speed:-f_speed),0);
		}else
		{
			c_rb.velocity = Vector2.zero;
		}
	}	

	public void Teleport()
	{
		if(direction == MoveDirection.Right ? gameObject.transform.position.x > f_limitX : gameObject.transform.position.x < -f_limitX)
		{
			if(!g_holeClone.activeInHierarchy)
			{				
				i_groundLvl = i_groundLvl + (direction == MoveDirection.Right?-1:1);
				i_groundLvl = (i_groundLvl < 0 ? 7 : i_groundLvl);
				i_groundLvl = (i_groundLvl > 7 ? 0 : i_groundLvl);

				g_holeClone.GetComponent<HoleMechanics>().i_groundLvl = i_groundLvl;
				g_holeClone.GetComponent<HoleMechanics>().direction = direction;
				g_holeClone.GetComponent<HoleMechanics>().b_activated = b_activated;

				g_holeClone.transform.localPosition = new Vector3((direction == MoveDirection.Right ? f_posX : -f_posX),GameManager.instance.f_highforgroundlvl[i_groundLvl],g_holeClone.transform.localPosition.z);
				
				g_holeClone.SetActive(true);
			}
		}

		if(direction == MoveDirection.Right ? gameObject.transform.position.x > (f_limitX+2): gameObject.transform.position.x < (-f_limitX-2))
		{
			gameObject.SetActive(false);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Hole" && col.gameObject.GetComponent<HoleMechanics>().b_activated && !b_activated)
		{			
			GameManager.instance.b_canSpanwHole = true;
			Debug.Log("no spawn");
		}else
		{
			b_activated = true;
		}
	}
}
