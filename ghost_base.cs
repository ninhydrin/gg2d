using UnityEngine;
using System.Collections;

public class ghost_base : Photon.MonoBehaviour
{
	public GameObject mapicon_ob;
	public GameObject ghostHP_ob;
	GameObject ghostHP;
	GameObject mapicon;
	RectTransform rt;
	Vector2 offset;
	ParticleSystem right;
	int myNum;
	public int  dominator;
	public Color domiColor;
	bool coF;

	GameObject myParent;
	ghost_control ghostC;
	For_next forNext;
	// Use this for initialization
	void Start ()
	{
		myParent = GameObject.Find("GhostList");
		transform.SetParent (myParent.transform);
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();	
PhotonTargets.MasterClient=
		ghostHP = Instantiate (ghostHP_ob);
		ghostHP.GetComponent<ghost_control> ().init (gameObject,forNext.playerNum);
		ghostHP.transform.SetParent (GameObject.Find ("Minimap").transform);

		right = transform.FindChild ("Light").GetComponent<ParticleSystem> ();
		offset.x = ((transform.position.x - 250f) * 3f) / 5f;
		offset.y = ((transform.position.z - 250f) * 3f) / 5f;
		
		
		ghostC = ghostHP.GetComponent<ghost_control> ();
		right.enableEmission = false;

		mapicon = Instantiate (mapicon_ob, offset, Quaternion.identity) as GameObject;	
		mapicon.transform.SetParent (GameObject.Find ("Map").transform);
		myNum = forNext.getGNum ();
		mapicon.GetComponent<ghost_icon> ().init (myNum, offset,this);

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (ghostC.dominator >= 0 && !coF && dominator != ghostC.dominator) {
			coF = true;
			StopCoroutine ("CanDominate");
			StartCoroutine ("CanDominate", ghostC.dominator);
		}
		if (dominator != ghostC.dominator) {
			SetParticle(false,-1,ghostC.domiColor);
		}
	}

	public void Damage (int da, int t)
	{
		photonView.RPC ("DamageRPC", PhotonTargets.All, new object[]{da,t});
		ghostHP.GetComponent<ghost_control> ().Damage (da, t);
	}

	public void SetDomi (int a)
	{
		dominator = a;
	}

	public int GetDomi ()
	{
		return dominator;
	}

	public void SetParticle (bool a, int c, Color b)
	{
		right.startColor = b;
		right.enableEmission = a;
		dominator = c;
		domiColor = b;

	}
	[PunRPC]
	void DamageRPC(int da,int t){
		ghostHP.GetComponent<ghost_control> ().Damage (da, t);	
	}

	public IEnumerator CanDominate (int a)
	{
		int k = 0;
		while (k<150) {
			if (ghostC.dominator != a) {
				coF = false;
				yield break;
			}
			k++;
			yield return 0;
		}
		SetParticle (true, a, ghostC.domiColor);
	}
}
