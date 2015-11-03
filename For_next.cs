using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class For_next : MonoBehaviour
{

	public int playerNum;
	public string[] mapMG;
	public string[] minimapMG;
	public string[] mapMaster;
	public string[] minimapMaster;
	public GameObject[] savaIcon;
	public bool[] ok;
	int myid;
	int id;
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
	public playerInfo[] players;
	// Use this for initialization
	void Start ()
	{
		players = new playerInfo[4];
		playerNum = 1;
		myid = -1;
		id = 0;
		ok = new bool[playerNum];
	}


	// Update is called once per frame
	void Update ()
	{
		if (!flag) {
			int count = 0;
			for (int i = 0; i<playerNum; i++) {
				if (ok [i])
					count++;
			}
			if (count == playerNum) {
				flag = true;
				StartCoroutine (LoadScene ());
			}
		}
	}

	public void SetPlayer (int pNum, GameObject pOb, Color pCo, int tNum, int cNum)
	{
	
		players [pNum] = new playerInfo (pOb, pCo, tNum, cNum);

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
