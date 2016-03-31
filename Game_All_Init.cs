using UnityEngine;
using System.Collections;

public class Game_All_Init : Photon.MonoBehaviour
{
	public GameObject ghost;
	public GameObject MGOb;
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

	int myNum;
	For_next forNext;
	int ghostIncome;
	int masterGhostIncome;
	GameObject[] playerMGs;
	Color[] playerColor;
	ghost_base[] ghosts;
	int[] ghostNum;
	bool myEnd;
	// Use this for initialization
	void Awake ()
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();				
		forNext.StartFlag(PhotonNetwork.player.ID);
		ghostList = GameObject.Find ("GhostList");
		myEnd = true;
	}

	void Update(){
		if (forNext.AllLoadEnd () && myEnd ) {
			Debug.Log ("non");
			myEnd = false;
			//StartInit ();
		}
	}
	void MakeMGHP(){
		MGHP = PhotonNetwork.Instantiate ("MG_HP", new Vector3 (-225f, -65f, 0), Quaternion.identity, 0) as GameObject;
		MGHP.tag = myNum.ToString()+"P_HP";
	}
	void MakeMG ()
	{
		MG = PhotonNetwork.Instantiate ("MG", new Vector3 (40+420*myNum, 0, 40+420*myNum), Quaternion.identity, 0) as GameObject;
		MG.transform.Rotate (new Vector3(-90f,45f+myNum*180f,0));
		//Player = PhotonNetwork.Instantiate ("UP", new Vector3 (50, 20, 50), Quaternion.identity, 0) as GameObject;
		//MakeMiniMG ();	
		//MakeMapMG ();
	}

	void MakeMiniMG ()
	{
		float x = (MG.transform.position.x - 250f) / 5f;
		float y = (MG.transform.position.z - 250f) / 5f;		
		minimapMG = PhotonNetwork.Instantiate ("miniMG", new Vector3 (x, y, 0), Quaternion.identity, 0) as GameObject;	
		minimapMG.GetComponent<minimap_MG> ().Init (mapMG);
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

	void StartInit(){
		organMap = GameObject.Find ("Organ/Map");
		miniMap = GameObject.Find ("Minimap/Field");
		MakeMG ();
		MakeMiniMG ();

	}
}
