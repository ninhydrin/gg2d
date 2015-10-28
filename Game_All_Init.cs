using UnityEngine;
using System.Collections;

public class Game_All_Init : MonoBehaviour
{
	public GameObject ghost;
	GameObject masterGhost;
	GameObject aGhost;
	int ghostIncome;
	int masterGhostIncome;
	GameObject tera;

	int playerNum=1;
	Color[] playerColor;

	ghost_base[] ghosts;
	int[] ghostNum;


	// Use this for initialization
	void Start ()
	{
		tera = GameObject.FindWithTag ("Terrain");
	
		int num = 1;
		ghosts = new ghost_base[9];
		for (int i = 0; i<3; i++) {
			for (int j=0; j<3; j++) {
				aGhost = Instantiate (ghost, new Vector3 (125 + i * 125, 0, 125 + j * 125), Quaternion.identity) as GameObject;
				ghosts[num-1]=aGhost.GetComponent<ghost_base>();
				aGhost.GetComponent<ghost_base> ().init (num);
				num++;
			}
		}
		ghostNum = new int[2];
	}
	
	// Update is called once per frame
	void Update ()
	{
		ghostNum = new int[2];
		for (int i = 0; i<9; i++) {
			if(ghosts[i].dominator>0){
				ghostNum[ghosts[i].dominator]++;
			}
		}
	}
	public void init(int pnum, Color[] pcolor){
		playerNum = pnum;
		playerColor = pcolor;
	}
	public int GetMyGhost(int a){
		return ghostNum [a];
	}
}
