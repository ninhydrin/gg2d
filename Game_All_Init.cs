using UnityEngine;
using System.Collections;

public class Game_All_Init : MonoBehaviour
{
	public GameObject ghost;
	public GameObject MGOb;

	GameObject masterGhost;
	GameObject aGhost;
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
	void Start ()
	{
		forNext=GameObject.Find("ForNextScene").GetComponent<For_next>();

		playerNum = forNext.playerNum;		
		playerMGs = new GameObject[playerNum];

		ghosts = new ghost_base[9];
		int num = 1;
		for (int i = 0; i<3; i++) {
			for (int j=0; j<3; j++) {
				aGhost = Instantiate (ghost, new Vector3 (125 + i * 125, 0, 125 + j * 125), Quaternion.identity) as GameObject;
				ghosts [num - 1] = aGhost.GetComponent<ghost_base> ();
				aGhost.GetComponent<ghost_base> ().init (num,playerNum);
				num++;
			}
		}
		masterGhost = GameObject.Find ("MGs");
		for (int i = 0; i<playerNum; i++) {
			playerMGs [i] = Instantiate (MGOb, new Vector3 (50 + 400 * i, 0, 50 + 400 * i), Quaternion.identity) as GameObject;
			playerMGs [i].transform.Rotate (Vector3.right * -90);
			playerMGs [i].GetComponent<MG_func>().init(i,forNext.players[i].playerColor);
			playerMGs [i].transform.SetParent (masterGhost.transform);
		}

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
