using UnityEngine;
using System.Collections;

public class For_next : MonoBehaviour
{

	public int playerNum;
	public GameObject[] mapMG;
	public GameObject[] minimapMG;
	public GameObject[] mapMaster;
	public GameObject[] minimapMaster;
	public GameObject[] savaIcon;
	

	public struct playerInfo
	{
		public GameObject playerOb;
		public Color playerColor;
		public int teamNum;

		public playerInfo (GameObject pOb, Color pCo, int tNum)
		{
			playerOb = pOb;
			playerColor = pCo;
			teamNum = tNum;
		}

	}
	public playerInfo[] players;
	// Use this for initialization
	void Start ()
	{
		players = new playerInfo[4];
		playerNum = 2;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public void SetPlayer(int pNum,GameObject pOb,Color pCo,int tNum){
		players [pNum] = new playerInfo (pOb, pCo, tNum);
	}
}
