using UnityEngine;
using System.Collections;

public class Game_All_Init : MonoBehaviour
{
	public GameObject ghost;
	public GameObject MGOb;
	public GameObject[] players;


	GameObject MGList;
	GameObject playerList;
	GameObject ghostList;
	GameObject miniMap;
	GameObject organMap;

	GameObject Player;
	GameObject aGhost;
	GameObject MG;
	GameObject mapMG;
	GameObject minimapMG;


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
		playerList=GameObject.Find("PlayerList");
		MGList = GameObject.Find ("MGList");
		ghostList = GameObject.Find ("GhostList");
		organMap = GameObject.Find("Organ/Map");
		miniMap = GameObject.Find("Minimap/Field");

		playerNum = forNext.playerNum;		

		MakeGhost();
		MakeMG ();
		MakeMapMG ();
		MakeMiniMG ();
		
	}


	void MakeMG(){
		MG = PhotonNetwork.Instantiate ("MG", new Vector3 (50, 0, 50), Quaternion.identity,0) as GameObject;
		MG.transform.Rotate (Vector3.right * -90);
		Player = PhotonNetwork.Instantiate ("UP", new Vector3 (50, 0, 50), Quaternion.identity, 0) as GameObject;
		Player.transform.SetParent (playerList.transform);
		MG.GetComponent<MG_func> ().init (Player, "Player", 0, forNext.players [0].playerColor);
		MG.transform.SetParent (MGList.transform);
	}

	void MakeMiniMG(){
		minimapMG = PhotonNetwork.Instantiate ("miniMG", new Vector3 (50, 0, 50), Quaternion.identity,0) as GameObject;		
		float x = (MG.transform.position.x - 250f) / 5f;
		float y = (MG.transform.position.z - 250f) / 5f;
		minimapMG.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
		minimapMG.transform.SetParent (miniMap.transform);
		minimapMG.GetComponent<RectTransform>().localScale = new Vector3 (1f,1f,0);		
	}
	void MakeMapMG(){
		mapMG = PhotonNetwork.Instantiate (forNext.mapMG [forNext.players [0].colorNum], Vector3.zero, Quaternion.identity, 0);
		mapMG.GetComponent<RectTransform>().localScale = new Vector3 (1f,1f,0);
		mapMG.GetComponent<organ_MG> ().init (MG);
		mapMG.transform.SetParent (organMap.transform);
	}
	void MakeGhost(){
		ghosts = new ghost_base[9];
		int num = 1;
		
		for (int i = 0; i<3; i++) {
			for (int j=0; j<3; j++) {
				aGhost = Instantiate (ghost, new Vector3 (125 + i * 125, 0, 125 + j * 125), Quaternion.identity) as GameObject;
				aGhost.transform.SetParent (ghostList.transform);
				ghosts [num - 1] = aGhost.GetComponent<ghost_base> ();
				aGhost.GetComponent<ghost_base> ().init (num, playerNum);
				num++;
			}
		}
	}

	// Update is called once per frame
	/*
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
	*/
}
