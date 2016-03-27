using UnityEngine;
using System.Collections;

public class Game_All_Init : Photon.MonoBehaviour
{
	public GameObject ghost;
	public GameObject MGOb;
	public GameObject[] players;
	public GameObject Mana;
	GameObject MGList;
	GameObject playerList;
	GameObject ghostList;
	GameObject miniMap;
	GameObject organMap;
	GameObject MGHP;
	GameObject Player;
	GameObject aGhost;
	GameObject MG;
	GameObject mapMG;
	GameObject minimapMG;
	Timelimit gameTimer;
	public commons common;

	int myNum;
	For_next forNext;
	int ghostIncome;
	int masterGhostIncome;
	GameObject tera;
	int playerNum;
	GameObject[] playerMGs;
	Color[] playerColor;
	ghost_base[] ghosts;
	int[] ghostNum;
	GameObject commonsList;

	// Use this for initialization
	void Awake ()
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();				
		commonsList = GameObject.Find ("commonsList");		
		foreach (Transform child in commonsList.transform){
			if(child.GetComponent<commons>().photonView.isMine) common = child.GetComponent<commons>();
		}
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		//myNum = forNext.owneerIdToNum [photonView.ownerId];
		myNum = forNext.myid;
		gameTimer = GameObject.Find ("Player_info/Time").GetComponent<Timelimit> ();

		ghostList = GameObject.Find ("GhostList");
		StartCoroutine (WaitInit ());
		/*
		organMap = GameObject.Find ("Organ/Map");
		miniMap = GameObject.Find ("Minimap/Field");
		playerNum = forNext.playerNum;

		if (PhotonNetwork.isMasterClient)
			MakeGhost ();
		MakeMG ();
		MakeMapMG ();
		MakeMiniMG ();
		*/

	}
	void MakeMGHP(){
		MGHP = PhotonNetwork.Instantiate ("MG_HP", new Vector3 (-225f, -65f, 0), Quaternion.identity, 0) as GameObject;
		MGHP.tag = myNum.ToString()+"P_HP";
	}
	void MakeMG ()
	{
		MG = PhotonNetwork.Instantiate ("MG", new Vector3 (40+420*myNum, 0, 40+420*myNum), Quaternion.identity, 0) as GameObject;
		MG.transform.Rotate (new Vector3(-90f,45f+myNum*180f,0));
		Player = PhotonNetwork.Instantiate ("UP", new Vector3 (50, 20, 50), Quaternion.identity, 0) as GameObject;
		MakeMiniMG ();	
		MakeMapMG ();
	}

	void MakeMiniMG ()
	{
		float x = (MG.transform.position.x - 250f) / 5f;
		float y = (MG.transform.position.z - 250f) / 5f;		
		minimapMG = PhotonNetwork.Instantiate ("miniMG", new Vector3 (x, y, 0), Quaternion.identity, 0) as GameObject;	
		/*
		float x = (MG.transform.position.x - 250f) / 5f;
		float y = (MG.transform.position.z - 250f) / 5f;
		minimapMG.transform.SetParent (GameObject.Find ("Minimap").transform);
		minimapMG.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
		minimapMG.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 0);		
		*/
	}
	void MakeMana(){
		Instantiate (Mana);
	}

	void MakeMapMG ()
	{
		mapMG = PhotonNetwork.Instantiate ("MapMG", Vector3.zero, Quaternion.identity, 0);
		mapMG.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 0);

	}

	void MakeGhost ()
	{
		ghosts = new ghost_base[9];
		int num = 1;
		
		for (int i = 0; i<3; i++) {
			for (int j=0; j<3; j++) {
				aGhost = PhotonNetwork.Instantiate ("Ghost", new Vector3 (125 + i * 125, 0, 125 + j * 125), Quaternion.identity,0) as GameObject;
				ghosts [num - 1] = aGhost.GetComponent<ghost_base> ();
				num++;
			}
		}
	}

	IEnumerator WaitInit(){
		int count = 0;
		common.ok = true;
		while (count != forNext.playerNum) {
			count=0;
			foreach (Transform child in commonsList.transform){
				if(child.GetComponent<commons>().ok) count++;
			}
			yield return 0;
		}
		organMap = GameObject.Find ("Organ/Map");
		miniMap = GameObject.Find ("Minimap/Field");
		playerNum = forNext.playerNum;
		
		if (PhotonNetwork.isMasterClient)
			MakeGhost ();
		MakeMG ();

		//MakeMiniMG ();
		common.myOk = true;
		gameTimer.IsEnable = true;
		
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
