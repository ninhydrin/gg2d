using UnityEngine;
using System.Collections;

public class Game_All_Init : MonoBehaviour
{
	public GameObject ghost;
	public GameObject MGOb;
	public GameObject[] players;
	GameObject masterGhost;
	GameObject Player;
	GameObject aGhost;
	GameObject Ghost;
	GameObject MG;
	For_next forNext;
	int ghostIncome;
	int masterGhostIncome;
	GameObject tera;
	int playerNum;
	GameObject[] playerMGs;
	Color[] playerColor;
	ghost_base[] ghosts;
	int[] ghostNum;
	

	// Use this for initialization
	void Awake ()
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		
		playerNum = forNext.playerNum;		
		Ghost = GameObject.Find ("Ghosts");

		ghosts = new ghost_base[9];
		int num = 1;
		/*
		for (int i = 0; i<3; i++) {
			for (int j=0; j<3; j++) {
				aGhost = Instantiate (ghost, new Vector3 (125 + i * 125, 0, 125 + j * 125), Quaternion.identity) as GameObject;
				aGhost.transform.SetParent (Ghost.transform);
				ghosts [num - 1] = aGhost.GetComponent<ghost_base> ();
				aGhost.GetComponent<ghost_base> ().init (num, playerNum);
				num++;
			}
		}*/
		masterGhost = GameObject.Find ("MGs");
		//Player = GameObject.Find("Player");
		MG = Instantiate (MGOb, new Vector3 (50, 0, 50), Quaternion.identity) as GameObject;
		Player = PhotonNetwork.Instantiate ("UP", new Vector3 (50, 0, 50), Quaternion.identity, 0) as GameObject;
		print ("ok");
		MG.transform.Rotate (Vector3.right * -90);
		MG.GetComponent<MG_func> ().init (Player, "Player", 0, forNext.players [0].playerColor);
		MG.transform.SetParent (masterGhost.transform);
	}
	
	// Update is called once per frame
	void Update ()
	{
		ghostNum = new int[2];
		for (int i = 0; i<9; i++) {
			if (ghosts [i].dominator > 0) {
				ghostNum [ghosts [i].dominator]++;
			}
		}
	}

	public int GetMyGhost (int a)
	{
		return ghostNum [a];
	}
}
