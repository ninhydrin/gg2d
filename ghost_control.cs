using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ghost_control : MonoBehaviour
{

	private Vector3 screenPoint;
	public GameObject myOb;
	private bool canShow;
	For_next forNext;
	public GameObject control_barOb;
	GameObject[] control_bar;
	private int[] Power;
	public int dominator;
	public Color domiColor;
	private float LenUnit;
	private float barLength;
	int playerNum;
	bool cando;

	void Start ()
	{
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
			for (int i=0; i<playerNum; i++) {
				if (Power [i] >= 100) {
					domiColor = Color.red;
					dominator = i;
				} else {
					dominator = -1;
				}
			}
		}
	}

	public void init (GameObject target, int pNum)
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		myOb = target;
		playerNum = pNum;

		control_bar = new GameObject[playerNum + 1];
		Power = new int[playerNum + 1];
		Power [playerNum] = 100;
		for (int i=0; i<playerNum+1; i++) {			
			control_bar [i] = Instantiate (control_barOb);			
				RectTransform rt = control_bar [i].GetComponent<RectTransform> ();
			if(i==0){
				rt.pivot = new Vector2(1f,0.5f);
				rt.anchoredPosition= Vector2.right*rt.sizeDelta.x/2;
			}else{
				rt.anchoredPosition= Vector2.left*rt.sizeDelta.x/2;				
			}
			control_bar [i].transform.SetParent (transform);
			control_bar [i].GetComponent<Image> ().color = forNext.players [i].playerColor;			
		}
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
		int sum = Power [playerNum];
		float yy = control_bar [playerNum].GetComponent<RectTransform> ().sizeDelta.y;
		
		for (int i =0; i<playerNum; i++) {
			control_bar [i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen (sum, LenUnit), yy);
			sum += Power [i];
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

	public void Damage (int a, int team)
	{
		Power [team] += a;
		if (Power [team] > 100)
			Power [team] = 100;
		if (Power [playerNum] < a) {
			Power [playerNum]=0;
		} else {
			Power [playerNum] -= a;
		}
		for (int i=0; i<playerNum; i++) {
			if(i!=team){				
				Power[i] = Power[i] < a ? 0:Power[i]-a/(playerNum-1);
			}
		}
	}

	void OnTriggerEnter (Collider collider)
	{

		print (collider);
		if (collider.gameObject.tag == "Ghost") {
				
		}
		if (collider.gameObject.tag == "Enemy_sava")
			collider.transform.parent.GetComponent<sava_base> ().Damage (120);

	}
}
