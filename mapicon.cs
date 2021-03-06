﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mapicon : MonoBehaviour
{
	public RectTransform rt;
	GameObject myOb;
	Sava_controler myObC;
	private Vector2 offset;
	GameObject cursor;
	RectTransform cursorRt;
	cursor_controller cursorC;
	float inter;
	bool imTarget;
	Image mycolor;
	organ_controller myorgan;
	int myGroupNum;
	For_next forNext;
	bool cando;
	// Use this for initialization
	void Start ()
	{
		transform.SetSiblingIndex (0);
	}
	
	// Update is called once per frame
	void Update ()
	{if (cando) {
			if (myOb == null) {
				Destroy (this.gameObject);
			} else {

				inter = Vector2.Distance (cursorRt.anchoredPosition, rt.anchoredPosition);

				if (inter < 8f && (cursorC.isTarget () <= 0 || (imTarget && cursorC.isTarget () == myGroupNum)) && !cursorC.isMoving ()) {
					if (!imTarget) {
						imTarget = true;
						StartCoroutine (OnCursor ());
					}
					cursorC.TargetingNum (myGroupNum);
					cursorC.MoveTo (rt.anchoredPosition, myGroupNum, gameObject);	

				} else if (inter > 6f) {
					if (imTarget)
						cursorC.TargetingNum ();
					imTarget = false;
				}
						

				iMove ();
			}
		}
	}

	public void init (GameObject nicon, int num=-100)
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();			
		myOb = nicon;
		myObC = nicon.GetComponent<Sava_controler> ();
		rt = GetComponent<RectTransform> ();
		offset.x = ((nicon.transform.position.x - 250f) * 3f) / 5f;
		offset.y = ((nicon.transform.position.z - 250f) * 3f) / 5f;
		rt.anchoredPosition = offset;
		if (num != -100)
			myGroupNum = num;	
		cursor = GameObject.Find ("Organ_Cursor");
		cursorC = cursor.GetComponent<cursor_controller> ();
		cursorRt = cursor.GetComponent<RectTransform> ();
		mycolor = gameObject.GetComponent<Image> ();
		myorgan = GameObject.FindWithTag (forNext.myid.ToString()+"P_Master").GetComponent<organ_controller> ();
		cando = true;
	}

	private IEnumerator OnCursor ()
	{
		Color or = mycolor.color;
		float maxcolor = 1f;
		float num = 0.05f;
		while (imTarget && myOb != null) {
			if (maxcolor < mycolor.color.r)
				num = -0.05f;
			else if (mycolor.color.r <= 0.5)
				num = 0.05f;
			mycolor.color = new Color (mycolor.color.r + num, mycolor.color.r + num, mycolor.color.r + num, 1f);
			yield return 10;        // 1フレーム後、再開
		}
		if (myOb != null)		
			mycolor.color = or;
	}

	public IEnumerator ImSelected ()
	{
		Vector2 or = rt.sizeDelta;
		float maxsize = 6f;
		float num = 0.7f;
		while (myorgan.savaDictSetting &&myOb !=null) {
			if (rt.sizeDelta.x - or.x > maxsize)
				num = -0.7f;
			else if (rt.sizeDelta.x - or.x <= 0)
				num = 0.7f;
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x + num, rt.sizeDelta.y + num);
			yield return 0;        // 1フレーム後、再開
		}
		if (myOb != null)
			transform.GetComponent<RectTransform> ().sizeDelta = or;		
	}

	public void SetDest (Vector2 dest)
	{
		myObC.SetDestination (dest);
	}

	public Vector2 GetDest ()
	{
		return myObC.GetDestination ();
	}

	public int getGroupNum ()
	{
		return myGroupNum;
	}

	public void SetGroupNum (int a)
	{
		myGroupNum = a;
		myObC.SetGnum(a);
	}

	void iMove ()
	{
		offset.x = ((myOb.transform.position.x - 250f) * 3f) / 5f;
		offset.y = ((myOb.transform.position.z - 250f) * 3f) / 5f;
		rt.anchoredPosition = offset;
	}

	public void SetLeader (bool a)
	{
		myObC.SetLeader (a);
	}

	public bool isLeader ()
	{
		return myObC.isLeader ();
	}

	public void EntrustLeader ()
	{
		myObC.EntrustLeader ();
	}
}
