using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class sava_head_HPLV: Photon.MonoBehaviour
{
	private Vector3 screenPoint;
	public GameObject myOb;
	private bool canShow;
	private GameObject HP_bar;
	private GameObject LV_bar;
	private RectTransform HPrt;
	private RectTransform LVrt;
	private bool cando;
	float HPLenUnit;
	int myObId;
	void Start ()
	{
		canShow = false;
		HP_bar = transform.FindChild ("HP").gameObject;
		LV_bar = transform.FindChild ("LV").gameObject;
		HPrt = HP_bar.GetComponent<RectTransform> ();
		LVrt = LV_bar.GetComponent<RectTransform> ();
	}

	void LateUpdate ()
	{	

		if (myOb != null) {
			Vector3 aa = myOb.transform.position;
			aa.y += 4f;
			screenPoint = Camera.main.WorldToScreenPoint (aa);
			HPrt.sizeDelta = new Vector2 (HPLenUnit * myOb.GetComponent<Sava_controler> ().HP, HPrt.sizeDelta.y);
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

	public void init (GameObject target,Color myC)
	{

		HP_bar = transform.FindChild ("HP").gameObject;
		LV_bar = transform.FindChild ("LV").gameObject;
		HPrt = HP_bar.GetComponent<RectTransform> ();
		LVrt = LV_bar.GetComponent<RectTransform> ();
		myOb = target;
		HPLenUnit = HP_bar.GetComponent<RectTransform> ().sizeDelta.x / (float)myOb.GetComponent<Sava_controler> ().maxHP;		
		myObId = target.GetComponent<PhotonView> ().viewID;
		HP_bar.GetComponent<Image> ().color = myC;
	}

	void HideBar ()
	{
		GetComponent<UnityEngine.UI.Image> ().enabled = false;
		HP_bar.GetComponent<UnityEngine.UI.Image> ().enabled = false;
		LV_bar.GetComponent<UnityEngine.UI.Image> ().enabled = false;
	}

	void ShowBar ()
	{
		GetComponent<UnityEngine.UI.Image> ().enabled = true;
		HP_bar.GetComponent<UnityEngine.UI.Image> ().enabled = true;
		LV_bar.GetComponent<UnityEngine.UI.Image> ().enabled = true;
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (myObId);
			
		} else {
			myObId = (int)stream.ReceiveNext ();
		}
	}
}

