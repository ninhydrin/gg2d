using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class For_next : MonoBehaviour
{
	
	public int playerNum;
	public string[] mapMG;
	public string[] minimapMG;
	public string[] mapMaster;
	public string[] minimapMaster;
	public GameObject[] savaIcon;
	int[] idToid;
	int myid;

	bool flag;

	public struct playerInfo
	{
		public GameObject playerOb;
		public Color playerColor;
		public int teamNum;
		public int colorNum;

		public playerInfo (GameObject pOb, Color pCo, int tNum, int cNum)
		{
			playerOb = pOb;
			playerColor = pCo;
			teamNum = tNum;
			colorNum = cNum;
		}

	}
	public Dictionary<int, playerInfo> players;
	public Dictionary<int, bool> ok;
	
	// Use this for initialization
	void Start ()
	{
		players = new Dictionary<int,playerInfo> ();
		ok = new Dictionary<int,bool> ();
		
		playerNum = 2;

	}


	// Update is called once per frame
	void Update ()
	{
		if (!flag) {
			int count = 0;
			PhotonPlayer [] player = PhotonNetwork.playerList;
			for (int i = 0; i < player.Length; i++) {
				if(ok.ContainsKey(player [i].ID))
					if(ok[player [i].ID])
						count++;
			}
			if (count == playerNum) {
				idToid = new int[playerNum];
				for (int i = 0; i < player.Length; i++) {
					idToid[i] = player[i].ID; 
				}
				System.Array.Sort(idToid);
				for (int i = 0; i < player.Length; i++) {
					if(idToid[i] == PhotonNetwork.player.ID){
						myid=i;
						break;
					}
				}
				flag = true;
				StartCoroutine (LoadScene ());
			}
		}
	}

	public void SetPlayer (int pNum, GameObject pOb, Color pCo, int tNum, int cNum)
	{
		players [pNum] = new playerInfo (pOb, pCo, tNum, cNum);
		ok [pNum] = true;
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
		//loadingBar.fillAmount = 1;
		
		yield return new WaitForSeconds (1);
		
		async.allowSceneActivation = true;    // シーン遷移許可
		
	}
}
