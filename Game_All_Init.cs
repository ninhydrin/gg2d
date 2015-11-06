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
	commons common;

	int myPNum;
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
		common = GameObject.FindWithTag("commons").GetComponent<commons> ();
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		myPNum = forNext.myid;
		common.ok[myPNum] = true;

		playerList = GameObject.Find ("PlayerList");
		MGList = GameObject.Find ("MGList");
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
		MGHP.tag = myPNum.ToString()+"P_HP";
	}
	void MakeMG ()
	{
		MG = PhotonNetwork.Instantiate ("MG", new Vector3 (40, 0, 40), Quaternion.identity, 0) as GameObject;
		MG.transform.Rotate (new Vector3(-90f,45f,0));
		Player = PhotonNetwork.Instantiate ("UP", new Vector3 (50, 0, 50), Quaternion.identity, 0) as GameObject;
		Player.transform.SetParent (playerList.transform);
		MG.transform.SetParent (MGList.transform);
	}

	void MakeMiniMG ()
	{
		minimapMG = PhotonNetwork.Instantiate ("miniMG", new Vector3 (50, 0, 50), Quaternion.identity, 0) as GameObject;		
		float x = (MG.transform.position.x - 250f) / 5f;
		float y = (MG.transform.position.z - 250f) / 5f;
		minimapMG.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
		minimapMG.GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 0);		
	}
	void MakeMana(){
		Instantiate (Mana);
	}

	void MakeMapMG ()
	{
		mapMG = PhotonNetwork.Instantiate (forNext.mapMG [forNext.players [0].colorNum], Vector3.zero, Quaternion.identity, 0);

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
		commons common = GameObject.FindWithTag ("commons").GetComponent<commons> ();
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();		
		int count = 0;
		while (count != forNext.playerNum) {
			count=0;
			for (int i=0;i<forNext.playerNum;i++){
				if(common.ok[i]) count++;
			}
			yield return 0;
		}
		organMap = GameObject.Find ("Organ/Map");
		miniMap = GameObject.Find ("Minimap/Field");
		playerNum = forNext.playerNum;
		
		if (PhotonNetwork.isMasterClient)
			MakeGhost ();
		MakeMG ();
		MakeMapMG ();
		MakeMiniMG ();
		common.myOk = true;
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
