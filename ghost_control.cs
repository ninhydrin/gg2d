﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ghost_control : Photon.MonoBehaviour
{

	private Vector3 screenPoint;
	public GameObject myOb;
	private bool canShow;
	For_next forNext;
	public GameObject control_barOb;
	GameObject[] control_bar;
	private float LenUnit;
	private float barLength;

	ghost_base ghostB;
	int playerNum;
	bool cando;

	void Start ()
	{
		GetComponent<RectTransform> ().localScale = new Vector3 (1, 1, 0);		
	}
	
	void LateUpdate ()
	{	
		if (cando) {
			if (myOb != null) {
				AdjustmentBar ();
				Vector3 aa = myOb.transform.position;
				aa.y += 4f;
				screenPoint = Camera.main.WorldToScreenPoint (aa);
				//screenPoint = Camera.main.WorldToScreenPoint (myOb.transform.position);
				if (screenPoint.z > 30 || screenPoint.z < 0)
					canShow = false;
				else
					canShow = true;
				if (canShow) {
					ShowBar ();
					transform.position = new Vector3 (screenPoint.x, screenPoint.y, 0);
				} else {
					HideBar ();
				}
			} else {
				HideBar ();
			}

		}
	}

	public void init (GameObject target,int pNum)
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		myOb = target;
		playerNum = forNext.playerNum;
		control_bar = new GameObject[playerNum + 1];
		ghostB = target.GetComponent<ghost_base> ();

		for (int i=0; i<=playerNum; i++) {			
			control_bar [i] = Instantiate (control_barOb);			
			RectTransform rt = control_bar [i].GetComponent<RectTransform> ();
			if (i == playerNum) {
				rt.pivot = new Vector2 (1f, 0.5f);
				rt.anchoredPosition = Vector2.right * rt.sizeDelta.x / 2;
				transform.SetAsFirstSibling();				
			} else {
				control_bar [i].GetComponent<Image> ().color = ghostB.domiColor[i];			
				rt.anchoredPosition = Vector2.left * rt.sizeDelta.x / 2;				
			}
			control_bar [i].transform.SetParent (transform);
		}
		control_bar [pNum].transform.SetAsLastSibling ();
		control_bar [playerNum].transform.SetAsFirstSibling ();
		canShow = false;
		barLength = control_bar [playerNum].GetComponent<RectTransform> ().sizeDelta.x;
		LenUnit = barLength / 100;
		cando = true;
	}

	void HideBar ()
	{
		GetComponent<UnityEngine.UI.Image> ().enabled = false;
		for (int i =0; i<playerNum+1; i++)
			control_bar [i].GetComponent<Image> ().enabled = false;
	}

	void ShowBar ()
	{
		GetComponent<UnityEngine.UI.Image> ().enabled = true;
		for (int i =0; i<playerNum+1; i++)		
			control_bar [i].GetComponent<Image> ().enabled = true;
	}

	void AdjustmentBar ()
	{
		int sum = 0;
		float yy = control_bar [playerNum].GetComponent<RectTransform> ().sizeDelta.y;
		
		for (int i =0; i<playerNum; i++) {
			sum += ghostB.power [i];			
			control_bar [i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen (sum, LenUnit), yy);
		}
	}

	float AdLen (int s, float l)
	{
		if (s * l > barLength) {
			return barLength;
		} else {
			return s * l;
		}
	}
/*
	public void Damage (int a, int team)
	{
		Power [team] += a;
		if (Power [team] > 100)
			Power [team] = 100;
		if (Power [playerNum] < a) {
			Power [playerNum] = 0;
		} else {
			Power [playerNum] -= a;
		}
		for (int i=0; i<playerNum; i++) {
			if (i != team) {				
				Power [i] = Power [i] < a ? 0 : Power [i] - a / (playerNum - 1);
			}
		}
	}
*/
}
