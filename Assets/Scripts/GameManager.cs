using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public int i_lives;
	public int i_lvl;
	public int i_holeNumber;
	public float[] f_highforgroundlvl;
	public float[] f_highforgroundlvlenemy;

	public GameObject[] g_holes;
	public GameObject[] g_spots;

	public bool b_canSpanwHole;
	int i_randomSpawn;
	public static GameManager instance;
	// Use this for initialization
	void Awake() {
		instance = this;
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(b_canSpanwHole)
		{
			SpawnHole();
		}
		
	}

	public void SpawnHole()
	{
		
		if(i_holeNumber < 8)
		{
			i_randomSpawn = Random.Range(0,g_holes.Length);
			g_holes[i_holeNumber].SetActive(true); 
			g_holes[i_holeNumber].GetComponent<HoleMechanics>().i_groundLvl = (int)((i_holeNumber)/3);
			g_holes[i_holeNumber].transform.position = g_spots[i_randomSpawn].transform.position;
			i_holeNumber++;
		}
		b_canSpanwHole = false;
	}
}
