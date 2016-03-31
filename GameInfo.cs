using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameInfo : MonoBehaviour
{
	public struct pInfoStruct
	{
		public int charNum;
		public int teamNum;
		public int colorNum;
		public Color myColor;

		public pInfoStruct (int chNum, int tNum, int cNum, Color mCo)
		{
			charNum = chNum;
			teamNum = tNum;
			colorNum = cNum;
			myColor = mCo;
		}
	}

	public Color[] Colors = new Color[6];
	public int playerNum;
	public Dictionary<int,pInfoStruct> playerInfo;

	public int myNum;
	public Color myColor;
	public int myTeam;
	public int myColorNum;
	void Start ()
	{
		DontDestroyOnLoad (gameObject);
		myNum = PhotonNetwork.player.ID;
		Colors [0] = new Color (1f, 8f/255, 8f/255);
		Colors [1] = new Color (37f/255, 75f/255, 1);
		Colors [2] = new Color (243f/255, 1, 41f/255);
		Colors [3] = new Color (220f/255, 59f/255, 1);
		Colors [4] = new Color (39f/255, 243f/255, 53f/255);
		Colors [5] = new Color (1, 1, 1);
	}


	public void SetInfo (int pNum, int cNum, int tNum)
	{	
		playerInfo [pNum] = new pInfoStruct (pNum, tNum, cNum, Colors [cNum]);
		if (pNum == PhotonNetwork.player.ID) {
			myNum = pNum;
			myColor = Colors [cNum];
			myTeam = tNum;		
			myColorNum = cNum;
		}
	}
}
