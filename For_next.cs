using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 
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
	public Dictionary <int,bool> startF;

	//Transform playerList;
	public Dictionary<int,pInfoStruct> roomPlayerDic;
	public Dictionary<int,bool> setOK;
	public pInfoStruct[] playerInfo;
	Room myRoom;
	// Use this for initialization
	void Start ()
	{
		//playerList = GameObject.Find ("Players").transform;
		playerNum = 2;//myRoom.maxPlayers;
		playerInfo = new pInfoStruct[playerNum];
		myid = -1;
		setOK = new Dictionary<int, bool> ();
		startF = new Dictionary<int, bool> ();
		numToOwnerId = new int[playerNum];
		numToColor = new Color[playerNum];
		ownerIdToNum = new Dictionary<int,int> ();
		roomPlayerDic = new Dictionary<int, pInfoStruct> ();
		myRoom = PhotonNetwork.room;
		DontDestroyOnLoad (gameObject);
		DontDestroyOnLoad (GameObject.Find ("PlayerList"));
		DontDestroyOnLoad (GameObject.Find ("MGList"));
		DontDestroyOnLoad (GameObject.Find ("GhostList"));
				
	}


	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.G))
			ForDebugg ();
		
		ready = AlignedPeople ();
		if (ready && !flag) {
			if (CheckPlayerReady ()) {
				flag = true;
				StartCoroutine (LoadScene ());
			}
		}
	}

	/// <summary>
	/// 全員ロードが終わったか
	/// </summary>
	public bool	AllLoadEnd ()
	{
		PhotonPlayer[] player = PhotonNetwork.playerList;
		foreach (PhotonPlayer pPlayer in player) {
			if (!startF [pPlayer.ID])
				return false;
		}
		return true;
	}

	/// <summary>
	/// ルームの規定人数が集まったかを確認し、全員に0から番号を振る
	/// </summary>

	bool AlignedPeople ()
	{
		if (PhotonNetwork.room != null) {
			if (PhotonNetwork.room.playerCount == playerNum) {
				if (!ready) {
					PhotonPlayer[] player = PhotonNetwork.playerList;
					foreach (PhotonPlayer p in player) {
						if (p.ID < 0)
							return false;
						setOK [p.ID] = false;
						startF [p.ID] = false;
						//roomPlayerDic [p.ID] = null;
					}
				}
				PhotonNetwork.room.open = false;
				return true;
			}
		}
			return false;
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
	void SF (int id)
	{
		startF [id] = true;
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
		foreach (PhotonPlayer p in player){
			if (!setOK [p.ID])
				return false;
		}
		return true;
	}

	/// <summary>
	/// 全員に自分の決定と情報を知らせる
	/// </summary>
	/// <param name='pNum'> 自分のID </param>
	/// <param name='tNum'> 自分のチーム番号 </param>
	/// <param name='cNum'> 自分のカラー番号 </param>

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
		PhotonNetwork.isMessageQueueRunning = false;
		Text loadingText = GameObject.Find ("Text").GetComponent<Text> ();
		AsyncOperation async = SceneManager.LoadSceneAsync("Main");
		
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
	
		yield return new WaitForSeconds (1);
		
		async.allowSceneActivation = true;    // シーン遷移許可
	}

	void ForDebugg ()
	{
		PhotonPlayer[] player = PhotonNetwork.playerList;
		foreach (PhotonPlayer p in player) {
			Debug.Log ("id is " + p.ID.ToString ());
			Debug.Log ("ok is " + setOK [p.ID].ToString());
			//Debug.Log ("pInfor is " + roomPlayerDic [p.ID].ToString());
		}
	}
}
