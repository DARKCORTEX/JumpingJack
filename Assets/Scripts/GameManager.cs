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
	public List<GameObject> enemylist;
	public GameObject[] g_spots;
	

	public bool b_canSpanwHole;
	public bool b_canSpawnEnemy;
	public bool b_canMoveHole;
	public bool b_canMoveEnemy;
	int i_randomSpawn;

	
	public static GameManager instance;
	// Use this for initialization
	void Awake() {
		instance = this;
		ShuffleList();
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(b_canSpanwHole)
		{
			SpawnHole();
		}
		
		if(b_canSpawnEnemy)
		{
			SpawnEnemy();
		}
	}

	public void SpawnHole()
	{
		
		if(i_holeNumber < 8)
		{
			if(!g_holes[i_holeNumber].GetComponent<HoleMechanics>().b_activated)
			{
				g_holes[i_holeNumber].SetActive(true);

				i_randomSpawn = Random.Range(0,g_spots.Length);
				g_holes[i_holeNumber].GetComponent<HoleMechanics>().i_groundLvl = (int)((i_randomSpawn)/3);
				g_holes[i_holeNumber].transform.position = g_spots[i_randomSpawn].transform.position; 
			}else
			{
				b_canSpanwHole = false;
				i_holeNumber++;
			}	
		}else{
			b_canSpanwHole = false;
		}

	}

	public void SpawnEnemy()
	{


	}

	public void ShuffleList()
	{
		for (int i = 0; i < enemylist.Count; i++) {
         GameObject temp = enemylist[i];
         int randomIndex = Random.Range(i, enemylist.Count);
         enemylist[i] = enemylist[randomIndex];
         enemylist[randomIndex] = temp;
     }
	}
}
