using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class charactor_select : Photon.MonoBehaviour
{

	int charNum = 3;
	int colorNum = 6;
	int teamNum=4;

	int seleP,seleC,seleT,selector;

	GameObject cam1;
	Image myColor,myTeam;
	public Sprite[] charFace = new Sprite[3];
	public GameObject[] charactors = new GameObject[2];
	public GameObject[] playerOb = new GameObject[3];
	public Color[] charColor = new Color[6];
	public int playerNum;
	Color barC=Color.gray;
	Image myFace;
	Button buttons;
	GameObject forNext;
	bool setOk;

	// Use this for initialization
	void Start ()
	{
		buttons = GetComponent<Button> ();
		myFace = GetComponent<Image> ();
		charNum = charFace.Length;
		cam1 = transform.FindChild("Cam").gameObject;
		myColor = transform.FindChild ("Color").GetComponent<Image> ();
		myTeam = transform.FindChild ("Team").GetComponent<Image> ();
		
		for (int i=0; i<charNum; i++) {
			charactors [i] = Instantiate (charactors [i], new Vector3 (i * 30,50*playerNum,0), Quaternion.identity) as GameObject;
		}
		forNext = GameObject.Find ("ForNextScene");
		DontDestroyOnLoad (forNext);

		myColor.color = charColor [seleC];
		SetCamPos ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (photonView.isMine) {
			if (!setOk) {
				SetCamPos ();
				if (selector == 0) {
					if (Input.GetKeyDown (buttons.LStick_Left)) {
						seleP = seleP < 1 ? charNum - 1 : seleP - 1;
						charactors [seleP].transform.eulerAngles = Vector3.zero;
						SetCamPos ();
					} else if (Input.GetKeyDown (buttons.LStick_Right)) {
						seleP = seleP > charNum - 2 ? 0 : seleP + 1;
						charactors [seleP].transform.eulerAngles = Vector3.zero;
						SetCamPos ();
					}
				} else if (selector == 1) {
					if (Input.GetKeyDown (buttons.LStick_Left)) {
						seleC = seleC < 1 ? colorNum - 1 : seleC - 1;
						myColor.color = charColor [seleC];
					} else if (Input.GetKeyDown (buttons.LStick_Right)) {
						seleC = seleC > colorNum - 2 ? 0 : seleC + 1;
						myColor.color = charColor [seleC];					
					}
				} else if (selector == 2) {
					if (Input.GetKeyDown (buttons.LStick_Left)) {
						seleT = seleT < 1 ? teamNum - 1 : seleT - 1;					
					} else if (Input.GetKeyDown (buttons.LStick_Right)) {
						seleT = seleT > teamNum - 2 ? 0 : seleT + 1;
					}
				}

				if (Input.GetKeyDown (buttons.LStick_Down)) {
					selector = selector < 1 ? 2 : selector - 1;				
				} else if (Input.GetKeyDown (buttons.LStick_Up)) {
					selector = selector > 1 ? 0 : selector + 1;		
				}

				for (int i =0; i<charNum; i++) {
					charactors [i].transform.Rotate (Vector3.up * Time.deltaTime * 10);
				}

				if (Input.GetButtonDown ("Jump")) {
					setOk = true;
					forNext.GetComponent<For_next> ().SetPlayer (playerNum, playerOb [seleP], charColor [seleC], teamNum);
					charactors [seleP].transform.eulerAngles = Vector3.zero;				
				}
			} else {
				if (Input.GetButtonDown ("Jump")) {
					StartCoroutine (LoadScene ());
					//Application.LoadLevel ("Main");
				}
				if (Input.GetKeyDown (KeyCode.L)) {
					setOk = false;
				}
			}
		}
	}

	void SetCamPos ()
	{
		myFace.sprite = charFace [seleP];
		cam1.transform.position = new Vector3 (30 * seleP, 2+50*playerNum,4);
	}
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//データの送信
			stream.SendNext(seleP);
			stream.SendNext(seleC);
			stream.SendNext(seleT);
			stream.SendNext(setOk);
			
		} else {
			//データの受信
			transform.position = (Vector3)stream.ReceiveNext();
			seleP=(int)stream.ReceiveNext();
			seleC=(int)stream.ReceiveNext();
			seleT=(int)stream.ReceiveNext();
			setOk=(bool)stream.ReceiveNext();
			
			
		}
	}
	
	IEnumerator LoadScene(){
		forNext.GetComponent<For_next> ().SetPlayer (3, playerOb [seleP], barC, 5);
		Text loadingText = GameObject.Find ("Text").GetComponent<Text> ();
		AsyncOperation async = Application.LoadLevelAsync("Main");
		async.allowSceneActivation = false;    // シーン遷移をしない
		
		while (async.progress < 0.9f) {
			Debug.Log(async.progress);
			loadingText.text = (async.progress * 100).ToString("F0") + "%";
			//loadingBar.fillAmount = async.progress;
			yield return new WaitForEndOfFrame();
		}
		
		Debug.Log("Scene Loaded");
		
		loadingText.text = "100%";
		//loadingBar.fillAmount = 1;
		
		yield return new WaitForSeconds(1);
		
		async.allowSceneActivation = true;    // シーン遷移許可
		
	}
	
}
