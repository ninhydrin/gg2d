using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class For_next : Photon.MonoBehaviour
{
	public struct pInfoStruct
	{
		public int charNum;
		public int teamNum;
		public int colorNum;

		public pInfoStruct (int chNum, int tNum, int cNum)
		{
			charNum = chNum;
			teamNum = tNum;
			colorNum = cNum;
		}
	}


	public int playerNum;
	//プレイヤーの数
	public Color[] Colors = new Color[6];
	public int[] numToOwnerId;
	public Color[] numToColor;
	public Dictionary<int, int> ownerIdToNum;
	public int myid;
	public Color myCo;
	int gnum = 0;
	public bool ready = false;
	public bool flag;
	public bool[] startF;

	Transform playerList;
	public pInfoStruct[] roomPlayerDic;
	public bool[] setOK;
	public pInfoStruct[] playerInfo;
	
	// Use this for initialization
	void Start ()
	{
		playerList = GameObject.Find ("Players").transform;
		playerNum = 2;
		playerInfo = new pInfoStruct[playerNum];
		myid = PhotonNetwork.player.ID;
		setOK = new bool[playerNum];
		startF = new bool[playerNum];

		numToOwnerId = new int[playerNum];
		numToColor = new Color[playerNum];
		ownerIdToNum = new Dictionary<int,int> ();

		DontDestroyOnLoad (gameObject);
		DontDestroyOnLoad (GameObject.Find ("PlayerList"));
		DontDestroyOnLoad (GameObject.Find ("MGList"));
		DontDestroyOnLoad (GameObject.Find ("GhostList"));
				
	}


	// Update is called once per frame
	void Update ()
	{
		ready = AlignedPeople (ready);
		if (ready && !flag) {
			if (CheckPlayerReady ()) {
				SetPlayerID ();
				flag = true;
				StartCoroutine (LoadScene ());
			}
		}
	}

	public bool	AllLoadEnd ()
	{
		int count = 0;
		for (int i = 0; i < playerNum; i++) {
			if (startF [i])
				count++;
		}
		if (count == playerNum) {
			return true;
		}
		return false;
	}

	/// <summary>
	/// ルームの規定人数が集まったかを確認し、全員に0から番号を振る
	/// </summary>
	/// <param name='ready'>
	/// すでに自身を呼び出したか
	/// </param>
	bool AlignedPeople (bool ready)
	{
		if (PhotonNetwork.room != null) {
			if (PhotonNetwork.room.playerCount == playerNum && !ready) {
				PhotonPlayer[] player = PhotonNetwork.playerList;
				for (int i = 0; i < player.Length; i++) {
					if (player [i].ID < 0)
						return false;
					setOK [i] = false;
				}
				if (PhotonNetwork.isMasterClient)
					SetPlayerNum ();
			}
		}
		return false;
	}

	/// <summary>
	/// 全員に固有の0,1の番号を振る
	/// </summary>
	void SetPlayerNum ()
	{
		roomPlayerDic = new pInfoStruct[playerNum];
		PhotonPlayer[] player = PhotonNetwork.playerList;
		for (int i = 0; i < player.Length; i++) {
			numToOwnerId [i] = player [i].ID;
			ownerIdToNum [player [i].ID] = i;
			object[] args = new object[2]{ i, player [i].ID };
			photonView.RPC ("SetID", PhotonTargets.All, args);
		}
	}


	[PunRPC]
	void SetID (int number, int id, PhotonMessageInfo info)
	{
		numToOwnerId [number] = id;
		ownerIdToNum [id] = number;
		if (PhotonNetwork.player.ID == id)
			myid = number;
	}

	public void StartFlag (int num)
	{
		photonView.RPC ("SF", PhotonTargets.All, num);
	}

	[PunRPC]
	void SF (int num)
	{
		startF [num] = true;
	}

	void SetPlayerID ()
	{
		PhotonPlayer[] player = PhotonNetwork.playerList;
		for (int i = 0; i < playerNum; i++) {			
			if (PhotonNetwork.isMasterClient) {
				object[] args = new object[2]{ i, player [i].ID };
				photonView.RPC ("SetID", PhotonTargets.All, args);
			}
			playerInfo [i] = roomPlayerDic [numToOwnerId [i]];
			if (numToOwnerId [i] == PhotonNetwork.player.ID) {
				myid = i;
				myCo = Colors [playerInfo [i].colorNum];
			}
		}
	
	}

	/// <summary>
	/// 全員が決定ボタンを押しているか
	/// </summary>
	/// <returns>
	/// 全員が決定しているか
	/// </returns>
	bool CheckPlayerReady ()
	{
		PhotonPlayer[] player = PhotonNetwork.playerList;
		for (int i = 0; i < player.Length; i++) {
			if (!setOK [i])
				return false;
		}
		return true;
	}

	public void SetPlayer (int pNum, int tNum, int cNum)
	{
		roomPlayerDic [pNum] = new pInfoStruct (pNum, tNum, cNum);
		object[] args = new object[4]{ pNum, cNum, tNum, true };
		photonView.RPC ("SendOK", PhotonTargets.All, args);
		setOK [pNum] = true;
	}

	public void UnsetPlayer (int pNum)
	{
		object[] args = new object[4]{ pNum, 0, 0, false };
		photonView.RPC ("SendOK", PhotonTargets.All, args);
		setOK [pNum] = false;
	}

	[PunRPC]
	public void SendOK (int pNum, int cNum, int tNum, bool un, PhotonMessageInfo info)
	{	
		roomPlayerDic [pNum] = new pInfoStruct (pNum, tNum, cNum);
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
