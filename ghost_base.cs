using UnityEngine;
using System.Collections;

public class ghost_base : Photon.MonoBehaviour
{
	public GameObject mapicon_ob;
	public GameObject ghostHP_ob;
	GameObject ghostHP;
	GameObject mapicon;
	PhotonView myPV;
	RectTransform rt;
	Vector2 offset;
	ParticleSystem right;
	int myNum, playerNum;
	public int  dominator;
	public Color[] domiColor;
	bool coF;
	GameObject myParent;
	ghost_control ghostC;
	For_next forNext;
	public int[] power;

	// Use this for initialization
	void Start ()
	{
		myParent = GameObject.Find ("GhostList");
		transform.SetParent (myParent.transform);
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		playerNum = forNext.playerNum;
		ghostHP = Instantiate (ghostHP_ob);
		ghostHP.GetComponent<ghost_control> ().init (gameObject, forNext.playerNum);
		ghostHP.transform.SetParent (GameObject.Find ("Minimap").transform);

		myPV = photonView;
		power = new int[playerNum + 1];
		domiColor = new Color[playerNum];
		power [playerNum] = 100;
		for (int i = 0; i<playerNum; i++)
			domiColor [i] = forNext.players [i].playerColor;

		right = transform.FindChild ("Light").GetComponent<ParticleSystem> ();
		offset.x = ((transform.position.x - 250f) * 3f) / 5f;
		offset.y = ((transform.position.z - 250f) * 3f) / 5f;
		
		ghostC = ghostHP.GetComponent<ghost_control> ();
		right.enableEmission = false;

		mapicon = Instantiate (mapicon_ob, offset, Quaternion.identity) as GameObject;	
		mapicon.transform.SetParent (GameObject.Find ("Map").transform);
		myNum = forNext.getGNum ();
		mapicon.GetComponent<ghost_icon> ().init (myNum, offset, this);
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*
		if (dominator >= 0 && !coF){
			coF = true;
			StopCoroutine ("CanDominate");
			StartCoroutine ("CanDominate", dominator);
		}
		if (dominator < 0) {
			SetParticle (false, -1, Color.black);
		}*/

	}

	[RPC]
	public void SyncPower (int[] ob, PhotonMessageInfo info)
	{
		bool domiCheck = true;
		for (int i=0; i<playerNum; i++) {
			power [i] = ob [i];			
			if (power [i] >= 100) {
				dominator = i;
				domiCheck = false;
			}
		}
		if (domiCheck) {
			dominator = -1;
			SetParticle (false, -1);			
		}
		if (dominator >= 0 && !coF) {
			coF = true;
			StopCoroutine ("CanDominate");
			StartCoroutine ("CanDominate", dominator);
		}
	}

	public void Damage (int da, int team)
	{
		power [team] += da;
		if (power [team] > 100)
			power [team] = 100;
		if (power [playerNum] < da) {
			power [playerNum] = 0;
		} else {
			power [playerNum] -= da;
		}
		for (int i=0; i<playerNum; i++) {
			if (i != team) {				
				power [i] = power [i] < da ? 0 : power [i] - da / (playerNum - 1);
			}
		}
		myPV.RPC ("SyncPower", PhotonTargets.All, power);		
	}

	public void SetDomi (int a)
	{
		dominator = a;
	}

	public int GetDomi ()
	{
		return dominator;
	}

	public void SetParticle (bool a, int c)
	{
		right.startColor = c >= 0 ? domiColor [c] : Color.black;
		right.enableEmission = a;
		dominator = c;
		coF = false;
	}

	public IEnumerator CanDominate (int a)
	{
		int k = 0;
		while (k<150) {
			if (dominator != a) {
				coF = false;
				yield break;
			}
			k++;
			yield return 0;
		}
		SetParticle (true, a);
	}
}
