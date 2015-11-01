using UnityEngine;
using System.Collections;

public class For_next : MonoBehaviour
{

	public int playerNum;
	public string[] mapMG;
	public string[] minimapMG;
	public string[] mapMaster;
	public string[] minimapMaster;
	public GameObject[] savaIcon;
	

	public struct playerInfo
	{
		public GameObject playerOb;
		public Color playerColor;
		public int teamNum;
		public int colorNum;

		public playerInfo ( GameObject pOb, Color pCo, int tNum,int cNum)
		{
			playerOb = pOb;
			playerColor = pCo;
			teamNum = tNum;
			colorNum = cNum;
		}

	}
	public playerInfo[] players;
	// Use this for initialization
	void Start ()
	{
		players = new playerInfo[4];
		playerNum = 6;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
	public void SetPlayer(int pNum,GameObject pOb,Color pCo,int tNum,int cNum){
		players [pNum] = new playerInfo (pOb, pCo, tNum,cNum);
	}
}
