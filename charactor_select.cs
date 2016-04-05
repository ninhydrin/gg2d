using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class charactor_select : Photon.MonoBehaviour
{

	int charNum = 3;
	int colorNum = 6;
	int teamNum = 4;
	int seleP, seleC, seleT, selector;
	GameObject cam1;
	Image myColor, myTeam;
	RectTransform rt;
	public Sprite[] charFace = new Sprite[3];
	public GameObject[] charactors = new GameObject[2];
	public GameObject[] playerOb = new GameObject[3];
	public Color[] charColor = new Color[6];
	public int playerNum;
	Color barC = Color.gray;
	Image myFace;
	Button buttons;
	For_next forNext;
	public bool setOk;
	public bool startOk;
	GameInfo gameInfo;
	// Use this for initialization
	void Start ()
	{
		transform.parent.SetParent (GameObject.Find ("Players").transform);
		buttons = GetComponent<Button> ();
		myFace = GetComponent<Image> ();
		charNum = charFace.Length;
		cam1 = transform.FindChild ("Cam").gameObject;
		myColor = transform.FindChild ("Color").GetComponent<Image> ();
		myTeam = transform.FindChild ("Team").GetComponent<Image> ();
		gameInfo = GameObject.Find ("GameMaster").GetComponent<GameInfo> ();
		rt = GetComponent<RectTransform> ();
		rt.anchoredPosition = new Vector2 (-180 + (photonView.ownerId - 1) * 360, 65);
		cam1.GetComponent<Camera> ().rect = new Rect (0.07f + (photonView.ownerId - 1) * 0.5f, 0, 0.3f, 0.5f);
		for (int i = 0; i < charNum; i++) {
			charactors [i] = Instantiate (charactors [i], new Vector3 (i * 30, 50 * photonView.ownerId, 0), Quaternion.identity) as GameObject;
		}

		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		DontDestroyOnLoad (forNext);
 
		myColor.color = forNext.Colors [seleC];
		SetCamPos ();
		
	}

	// Update is called once per frame
	void Update ()
	{
		SetCamPos ();
		//myColor.color = charColor [seleC];
		if (photonView.isMine) {
			if (!forNext.ready)
				setOk = false;

			if (!setOk) {
				OnKeyDown ();
				MoveCharactor ();

				if (Input.GetButtonDown ("Jump") && forNext.ready) {
					forNext.SetPlayer (PhotonNetwork.player.ID, gameInfo.myNum, teamNum, seleC);
					charactors [seleP].transform.eulerAngles = Vector3.zero;
					setOk = true;
				}
			} else {
				if (!forNext.flag) {
					if (Input.GetKeyDown (KeyCode.L)) {
						forNext.UnsetPlayer (PhotonNetwork.player.ID);
						setOk = false;
					}
				}
			}
		}
		
	}

	void SetCamPos ()
	{
		myFace.sprite = charFace [seleP];
		cam1.transform.position = new Vector3 (30 * seleP, 2 + 50 * photonView.ownerId, 4);
	}

	void MoveCharactor ()
	{
		for (int i = 0; i < charNum; i++) {
			charactors [i].transform.Rotate (Vector3.up * Time.deltaTime * 10);
		}
	}

	void OnKeyDown ()
	{
		if (selector == 0) { //キャラクター
			if (Input.GetKeyDown (buttons.LStick_Left)) {
				seleP = seleP < 1 ? charNum - 1 : seleP - 1;
				charactors [seleP].transform.eulerAngles = Vector3.zero;
			} else if (Input.GetKeyDown (buttons.LStick_Right)) {
				seleP = seleP > charNum - 2 ? 0 : seleP + 1;
				charactors [seleP].transform.eulerAngles = Vector3.zero;
			}
		} else if (selector == 1) {//カラー
			if (Input.GetKeyDown (buttons.LStick_Left)) {
				seleC = seleC < 1 ? colorNum - 1 : seleC - 1;
			} else if (Input.GetKeyDown (buttons.LStick_Right)) {
				seleC = seleC > colorNum - 2 ? 0 : seleC + 1;
			}
			myColor.color = forNext.Colors [seleC];
		} else if (selector == 2) {//チーム
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

	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			//データの送信
			stream.SendNext (transform.position);
			stream.SendNext (seleP);
			stream.SendNext (seleC);
			stream.SendNext (seleT);
			stream.SendNext (setOk);

			
		} else {
			//データの受信
			transform.position = (Vector3)stream.ReceiveNext ();
			seleP = (int)stream.ReceiveNext ();
			seleC = (int)stream.ReceiveNext ();
			seleT = (int)stream.ReceiveNext ();
			setOk = (bool)stream.ReceiveNext ();
			
		}
	}
	
}
