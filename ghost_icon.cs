using UnityEngine;
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
	Sprite[] domiColorList;
	Sprite neutralColor;
	For_next forNext;
	bool cando;
	// Use this for initialization
	void Start ()
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();		
		organC = GameObject.FindWithTag (forNext.myid.ToString()+"P_Master").GetComponent<organ_controller> ();
		cursorC = GameObject.Find ("Organ/Map/Organ_Cursor").GetComponent<cursor_controller> ();
		cursorRt = GameObject.Find ("Organ/Map/Organ_Cursor").GetComponent<RectTransform> ();
		myColor = GetComponent<Image> ();
		transform.SetSiblingIndex (0);
		domiColorList = Resources.LoadAll<Sprite>("G");
		neutralColor = Resources.Load<Sprite> ("GH");
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (cando) {
			myColor.sprite = ghostB.dominator >= 0 ? domiColorList[ghostB.dominator] : neutralColor;
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
	}

	public void init (int num, Vector2 offset, ghost_base a)
	{

		myNum = num;
		transform.FindChild ("Number").GetComponent<UnityEngine.UI.Text> ().text = num.ToString ();
		rt = transform.GetComponent<RectTransform> ();		
		rt.anchoredPosition = offset;
		ghostB = a;
		myGroupNum = -10 - num;
		cando = true;
	}

}
