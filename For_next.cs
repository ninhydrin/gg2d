using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class For_next : Photon.MonoBehaviour
{
	
	public int playerNum;
	//プレイヤーの数
	public Color[] Colors = new Color[6];
	public string[] minimapMaster;
	public GameObject[] savaIcon;
	public int[] numToOwnerId;
	public Color[] numToColor;
	public Dictionary<int, int> owneerIdToNum;
	public int myid;
	public Color myCo;
	int gnum = 0;
	public bool ready = false;
	bool flag;

	public struct pInfoStruct
	{
		public GameObject playerOb;
		public Color playerColor;
		public int teamNum;
		public int colorNum;

		public pInfoStruct (GameObject pOb, Color pCo, int tNum, int cNum)
		{
			playerOb = pOb;
			playerColor = pCo;
			teamNum = tNum;
			colorNum = cNum;
		}
	}

	Transform playerList;
	public Dictionary<int, pInfoStruct> roomPlayerDic;
	public Dictionary<int,bool> setOK;
	public pInfoStruct[] players;
	
	// Use this for initialization
	void Start ()
	{
		playerList = GameObject.Find ("Players").transform;
		playerNum = 2;
		players = new pInfoStruct[playerNum];
		setOK = new Dictionary<int, bool> ();
		numToOwnerId = new int[playerNum];
		numToColor = new Color[playerNum];
		owneerIdToNum = new Dictionary<int,int> ();
		DontDestroyOnLoad (gameObject);
		DontDestroyOnLoad (GameObject.Find ("PlayerList"));
		DontDestroyOnLoad (GameObject.Find ("MGList"));
		DontDestroyOnLoad (GameObject.Find ("GhostList"));
				
	}


	// Update is called once per frame
	void Update ()
	{
		
		if (PhotonNetwork.room != null) {
			if (PhotonNetwork.room.playerCount == playerNum && !ready) {
				roomPlayerDic = new Dictionary<int,pInfoStruct> ();
				PhotonPlayer[] player = PhotonNetwork.playerList;
				for (int i = 0; i < player.Length; i++) {
					setOK [player [i].ID] = false;
				}
				ready = true;
			} else if (PhotonNetwork.room.playerCount != playerNum) { 
				PhotonPlayer[] player = PhotonNetwork.playerList;
				for (int i = 0; i < player.Length; i++) {
					setOK [player [i].ID] = false;
				}
				ready = false;
			}
		}
		if (ready && !flag) {
			int count = 0;
			PhotonPlayer[] player = PhotonNetwork.playerList;
			for (int i = 0; i < player.Length; i++) {
				if (setOK [player [i].ID])
					count++;
			}
			if (count == playerNum) {
				for (int i = 0; i < player.Length; i++) {
					numToOwnerId [i] = player [i].ID; 
				}
				System.Array.Sort (numToOwnerId);
				for (int i = 0; i < player.Length; i++) {
					players [i] = roomPlayerDic [numToOwnerId [i]];
					owneerIdToNum [numToOwnerId [i]] = i;
					if (numToOwnerId [i] == PhotonNetwork.player.ID) {
						myid = i;
						myCo = players [i].playerColor;
					}
				}
				flag = true;
				StartCoroutine (LoadScene ());
			}
		}
	}

	public int aa = 110;

	public void SetPlayer (int pNum, GameObject pOb, int tNum, int cNum)
	{
		roomPlayerDic [pNum] = new pInfoStruct (pOb, Colors [cNum], tNum, cNum);
		object[] args = new object[4]{ pNum, cNum, tNum, true};
		photonView.RPC ("SendOK", PhotonTargets.All, args);
		setOK [pNum] = true;
	}

	public void UnsetPlayer (int pNum)
	{
		object[] args = new object[4]{ pNum, 0, 0, false};
		photonView.RPC ("SendOK", PhotonTargets.All, args);
		setOK [pNum] = false;
	}

	[PunRPC]
	public void SendOK (int pNum,int cNum,int tNum,bool un,PhotonMessageInfo info)
	{		
		setOK [pNum] = un ? true : false;

	}

	public int getGNum ()
	{
		gnum++;
		return gnum - 1;
	}

	IEnumerator LoadScene ()
	{
		Text loadingText = GameObject.Find ("Text").GetComponent<Text> ();
		AsyncOperation async = Application.LoadLevelAsync ("Main");
		

		async.allowSceneActivation = false;    // シーン遷移をしない
		
		while (async.progress < 0.9f) {
			Debug.Log (async.progress);
			loadingText.text = (async.progress * 100).ToString ("F0") + "%";
			//loadingBar.fillAmount = async.progress;
			yield return new WaitForEndOfFrame ();
		}
		
		Debug.Log ("Scene Loaded");


		loadingText.text = "100%";
	
		PhotonPlayer[] player = PhotonNetwork.playerList;
	
		//loadingBar.fillAmount = 1;

		yield return new WaitForSeconds (1);
		
		async.allowSceneActivation = true;    // シーン遷移許可
	}


}
