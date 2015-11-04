using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class organ_MG : MonoBehaviour
{
	
	public RectTransform rt;
	private GameObject MG;
	private Vector2 offset;
	public GameObject map;
	GameObject myParent;
	GameObject cursor;
	RectTransform cursorRt;
	cursor_controller cursorC;
	bool imTarget;
	Image mycolor;
	organ_controller organC;
	int myGroupNum;
	For_next forNext;
	bool startFlag;

	bool cando;
	// Use this for initialization
	void Start ()
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();		
		rt = GetComponent<RectTransform> ();
		rt.localScale = new Vector3 (1f, 1f, 0);
		MG = GameObject.FindWithTag (forNext.myid.ToString () + "P_MG");
		organC = GameObject.FindWithTag (forNext.myid.ToString () + "P_Master").GetComponent<organ_controller> ();
		StartCoroutine(WaitInit())
		offset = rt.anchoredPosition;
		offset.x = (MG.transform.position.x - 250f) * 0.6f;
		offset.y = (MG.transform.position.z - 250f) * 0.6f;
		rt.anchoredPosition = offset;
		cursor = GameObject.Find ("Organ_Cursor");
		cursorC = cursor.GetComponent<cursor_controller> ();
		cursorRt = cursor.GetComponent<RectTransform> ();
		imTarget = false;
		mycolor = gameObject.GetComponent<Image> ();
		myGroupNum = -10;
		transform.SetSiblingIndex (0);
	}

	void Update ()
	{
		if (cando) {
			if (organC.savaDictSetting || organC.summoning) {
				float inter = Vector2.Distance (cursorRt.anchoredPosition, rt.anchoredPosition);

				if (inter < 15f && (cursorC.isTarget () == 0 || (imTarget && cursorC.isTarget () == -10)) && !cursorC.isMoving ()) {
					if (!imTarget) {
						imTarget = true;
						StartCoroutine (OnCursor ());
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
	IEnumerator WaitInit(){
		commons common = GameObject.FindWithTag ("commons").GetComponent<commons> ();
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();		
		while (common.ok[forNext.myid]) {
			yield return 0;
		}
		myParent = GameObject.Find ("Organ/Map");
		transform.SetParent (myParent.transform);
		cando = true;
	}

	private IEnumerator OnCursor ()
	{
		Color or = mycolor.color;
		float maxcolor = 1f;
		float num = 0.05f;
		while (imTarget) {
			if (maxcolor < mycolor.color.r)
				num = -0.05f;
			else if (mycolor.color.r <= 0.5)
				num = 0.05f;
			mycolor.color = new Color (mycolor.color.r + num, mycolor.color.r + num, mycolor.color.r + num, 1f);
			yield return 10;        // 1フレーム後、再開
		}
		
		mycolor.color = or;
	}

	private IEnumerator ImSelected ()
	{
		Vector2 or = rt.sizeDelta;
		float maxsize = 10f;
		float num = 0.5f;
		while (imTarget) {
			if (rt.sizeDelta.x - or.x > maxsize)
				num = -num;
			else if (rt.sizeDelta.x - or.x < -maxsize)
				num = -num;
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x + num, rt.sizeDelta.y + num);
			yield return 0;        // 1フレーム後、再開
		}
		
		transform.GetComponent<RectTransform> ().sizeDelta = or;
	
	}

}
