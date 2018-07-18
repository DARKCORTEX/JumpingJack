using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

	public int i_lives;
	public int i_lvl;
	public int i_holeNumber;
	public int i_score;
	public int i_highscore;

	public GameObject g_player;
	public float[] f_highforgroundlvl;
	public float[] f_highforgroundlvlenemy;

	public GameObject[] g_holes;
	public List<GameObject> l_enemyList;
	public GameObject[] g_spots;
	public GameObject[] g_lives;
	

	public bool b_canSpanwHole;
	public bool b_canSpawnEnemy;
	public bool b_canMoveHole;
	public bool b_canMoveEnemy;
	public bool b_canMovePlayer;
	public bool b_canNextHazard;
	int i_randomSpawn;

	public GameObject g_nextHazard;
	public GameObject g_extraLife;
	public Text t_nextHazardCounter;
	public Text t_storyText;
	public string[] s_storyComplete;


	public GameObject g_deadScreen;
	public GameObject g_deadScore;
	public Text t_finalScoreAndLvlText;

	public Text t_score;
	public Text t_highscore;

	public GameObject g_intro;
	public static GameManager instance;
	// Use this for initialization
	void Awake() {
		instance = this;
		ShuffleList();
	}
	void Start () {
		
	}
	
	void Update()
	{
		if(i_lives == 0)
		{
			if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
			{
				ResetWorldDead();
			}
		}

		if(g_nextHazard.activeInHierarchy)
		{
			if((Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return)) && i_lvl < 21)
			{
				ResetWorldNext();
			}else
			{
				ResetWorldDead();
			}
		}

		if(g_intro.activeInHierarchy)
		{
			if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
			{
				g_intro.SetActive(false);
				b_canMoveHole = true;
				b_canMoveEnemy = true;
				b_canMovePlayer = true;
				b_canNextHazard = true;
				i_lives = 7;
				i_lvl = 0;
				i_holeNumber = 2;
			}
		}
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

	public void ShuffleList()
	{
		for (int i = 0; i < l_enemyList.Count; i++) {
         GameObject temp = l_enemyList[i];
         int randomIndex = Random.Range(i, l_enemyList.Count);
         l_enemyList[i] = l_enemyList[randomIndex];
         l_enemyList[randomIndex] = temp;
     	}
	}

	public void NextHazard()
	{
		if(b_canNextHazard)
		{
			g_extraLife.SetActive(false);
			b_canNextHazard = false;
			b_canMoveHole = false;
			b_canMoveEnemy = false;
			b_canMovePlayer = false;

			
			i_lvl++;
			

			g_nextHazard.SetActive(true);
			t_nextHazardCounter.text = "NEXT LEVEL - "+i_lvl+" HAZARD";
			t_storyText.text = s_storyComplete[i_lvl];

			if(i_lvl == 6 || i_lvl == 11 || i_lvl == 16)
			{
				g_extraLife.SetActive(true);
				i_lives++;
				UpdateLives();
			}
		}
	}

	public void UpdateLives()
	{
		foreach(GameObject go in g_lives)
		{
			go.SetActive(false);
		}
		for(int i = 0; i < i_lives;i++)
		{
			g_lives[i].SetActive(true);
		}

		if(i_lives == 0)
		{
			g_deadScreen.SetActive(true);
			if(i_score > i_highscore)
			{
				i_highscore = i_score;
				t_highscore.text = "HI"+i_highscore;
				g_deadScore.SetActive(true);
			}

			b_canMoveHole = false;
			b_canMoveEnemy = false;
			b_canMovePlayer = false;
		}
	}

	public void ResetWorldDead()
	{
		i_lives = 7;
		i_lvl = 0;
		i_score = 0;
		t_score.text = "SC0";

		g_player.GetComponent<PlayerMechanics>().b_stunned = false;
		g_player.GetComponent<PlayerMechanics>().b_falling = false;
		g_player.transform.position = new Vector2(0,-3.535f);
		g_player.GetComponent<BoxCollider2D>().isTrigger = false;

		ResetHoles();
		ResetEnemies();

		ShuffleList();

		UpdateLives();

		b_canMoveHole = true;
		b_canMoveEnemy = true;
		b_canMovePlayer = true;
		b_canNextHazard = true;

		g_deadScore.SetActive(false);
		g_deadScreen.SetActive(false);

		
	}

	public void ResetWorldNext()
	{
		ResetHoles();
		i_lives++;
		i_lvl++; 
		g_player.GetComponent<PlayerMechanics>().b_falling = false;
		g_player.GetComponent<Animator>().ResetTrigger("Fall");
		g_player.GetComponent<PlayerMechanics>().b_stunned = false;
		g_player.transform.position = new Vector2(0,-3.535f);
		g_player.GetComponent<BoxCollider2D>().isTrigger = false;

		if(i_lvl != 0)
		{
			l_enemyList[i_lvl-1].SetActive(true);
		}
		b_canMoveHole = true;
		b_canMoveEnemy = true;
		b_canMovePlayer = true;
		b_canNextHazard = true;

		g_nextHazard.SetActive(false);
	}

	public void ResetHoles()
	{
		i_holeNumber = 2;
		foreach(GameObject go in g_holes)
		{
			go.GetComponent<HoleMechanics>().g_holeClone.SetActive(false);
			go.GetComponent<HoleMechanics>().g_holeClone.GetComponent<HoleMechanics>().b_activated = false;
			go.GetComponent<HoleMechanics>().b_activated = false;
			go.SetActive(false);
		}

		for(int i = 0; i < i_holeNumber;i++)
		{
			g_holes[i].SetActive(true);
			g_holes[i].GetComponent<HoleMechanics>().b_activated = true;
		}
	}

	public void ResetEnemies()
	{
		foreach(GameObject go in l_enemyList)
		{
			go.SetActive(false);
		}
	}

	public void ScoreUp()
	{
		i_score += (i_lvl+1)*5;
		t_score.text = "SC"+i_score; 
		t_finalScoreAndLvlText.text = "FINAL SCORE "+ i_score + "\n WITH "+i_lvl+" HAZARDS";

	}
}
