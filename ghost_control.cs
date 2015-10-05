using UnityEngine;
using System.Collections;

public class ghost_control : MonoBehaviour
{

	private Vector3 screenPoint;
	public GameObject myOb;
	private bool canShow;
	private GameObject control_bar;
	private GameObject control_barA;
	private GameObject control_barB;
	private GameObject control_barC;
	private GameObject control_barD;
	private GameObject control_barE;
	private GameObject control_barF;

	private int Power;
	private int PowerA;
	private int PowerB;
	private int PowerC;
	private int PowerD;
	private int PowerE;
	private int PowerF;

	public int dominator;
	public Color domiColor;
	private float LenUnit;
	private float barLength;
	void Start ()
	{
		control_bar = transform.FindChild ("Ghost_control_bar").gameObject;
		control_barA = transform.FindChild ("Ghost_control_bar_A").gameObject;
		control_barB = transform.FindChild ("Ghost_control_bar_B").gameObject;
		control_barC = transform.FindChild ("Ghost_control_bar_C").gameObject;
		control_barD = transform.FindChild ("Ghost_control_bar_D").gameObject;
		control_barE = transform.FindChild ("Ghost_control_bar_E").gameObject;
		control_barF = transform.FindChild ("Ghost_control_bar_F").gameObject;
		canShow = false;
		Power = 100;
		barLength = control_bar.GetComponent<RectTransform> ().sizeDelta.x;
		LenUnit = barLength / 100;

	}
	
	void LateUpdate ()
	{	

		if (myOb != null) {
			AdjustmentBar();
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
		if (PowerA >= 100) {
			domiColor = Color.red;
			dominator=1;
		} else {
			dominator=-1;
		}

	}

	public void init (GameObject target)
	{
		myOb = target;
	}

	void HideBar ()
	{
		GetComponent<UnityEngine.UI.Image> ().enabled = false;
		control_bar.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		control_barA.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		control_barB.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		control_barC.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		control_barD.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		control_barE.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		control_barF.GetComponent<UnityEngine.UI.Image> ().enabled = false;

		
	}

	void ShowBar ()
	{
		GetComponent<UnityEngine.UI.Image> ().enabled = true;
		control_bar.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		control_barA.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		control_barB.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		control_barC.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		control_barD.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		control_barE.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		control_barF.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		
	}
	void AdjustmentBar(){
		int sum = PowerA;
		float yy = control_bar.GetComponent<RectTransform> ().sizeDelta.y;
		control_barA.GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen(sum,LenUnit), yy);
		sum += PowerB;
		control_barB.GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen(sum,LenUnit), yy);
		sum += PowerC;
		control_barC.GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen(sum,LenUnit), yy);
		sum += PowerD;
		control_barD.GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen(sum,LenUnit), yy);
		sum += PowerE;
		control_barE.GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen(sum,LenUnit), yy);
		sum += PowerF;
		control_barF.GetComponent<RectTransform> ().sizeDelta = new Vector2 (AdLen(sum,LenUnit), yy);
	}
	float AdLen(int s,float l){
		if (s * l > barLength) {
			return barLength;
		} else {
			return s*l;
		}
	}
	public void Damage (int a,int team)
	{
		if (team == 1) {
			PowerA+=a;
			if(Power<a){
				a-=Power;
			}else{
				Power-=a;
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
