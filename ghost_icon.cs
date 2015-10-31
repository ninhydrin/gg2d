﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ghost_icon : MonoBehaviour
{
	int myNum;
	RectTransform rt;
	ghost_base ghostB;
	Image myColor;
	organ_controller organC;
	cursor_controller cursorC;
	RectTransform cursorRt;
	bool imTarget;
	int myGroupNum;
	Color defaultColor = Color.gray;
	// Use this for initialization
	void Start ()
	{
		rt = transform.GetComponent<RectTransform> ();
		organC = GameObject.FindWithTag ("Player").GetComponent<organ_controller> ();
		cursorC = GameObject.Find ("Organ/Map/Organ_Cursor").GetComponent<cursor_controller> ();
		cursorRt = GameObject.Find ("Organ/Map/Organ_Cursor").GetComponent<RectTransform> ();
		myColor = GetComponent<Image> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		myColor.color = ghostB.dominator >= 0 ? ghostB.domiColor : defaultColor;

		if (organC.savaDictSetting || organC.summoning) {
			float inter = Vector2.Distance (cursorRt.anchoredPosition, rt.anchoredPosition);

			if (inter < 15f && (cursorC.isTarget () == 0 || (imTarget && cursorC.isTarget () == myGroupNum)) && !cursorC.isMoving ()) {
				if (!imTarget) {
					imTarget = true;
				}
				cursorC.TargetingNum (myGroupNum);
				cursorC.MoveTo (rt.anchoredPosition, myGroupNum, gameObject);		
				
			} else if (inter > 15f) {
				if (imTarget)
					cursorC.TargetingNum ();
				imTarget = false;
			}
		} else {
			imTarget = false;
		}
	}

	public void init (int num, Vector2 offset, ghost_base a)
	{
		rt = transform.GetComponent<RectTransform> ();
		myNum = num;
		transform.FindChild ("Number").GetComponent<UnityEngine.UI.Text> ().text = num.ToString ();
		rt.anchoredPosition = offset;
		ghostB = a;
		myGroupNum = -10 - num;
	}

}
