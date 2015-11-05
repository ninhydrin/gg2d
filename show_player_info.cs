using UnityEngine;
using System.Collections;

public class show_player_info : MonoBehaviour
{
	GameObject myPlayer;
	PlayerController pcon;
	RectTransform myHP;
	Transform myHPText;
	RectTransform myTP;
	float HPLenUnit;
	float TPLenUnit;
	For_next forNext;

	private bool cando;
	// Use this for initialization
	void Start ()
	{
		StartCoroutine (WaitInit ());
	}
	IEnumerator WaitInit(){
		commons common = GameObject.FindWithTag ("commons").GetComponent<commons> ();
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();		
		while (!common.myOk) {
			yield return 0;
		}	
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();		
		myPlayer = GameObject.FindWithTag (forNext.myid+"P_Master");
		pcon = myPlayer.GetComponent<PlayerController> ();
		myHP = transform.FindChild ("Player_HP").GetComponent<RectTransform> ();
		myTP = transform.FindChild ("Player_TP").GetComponent<RectTransform> ();
		myHPText = transform.FindChild ("Player_HP_Text");
		HPLenUnit = myHP.sizeDelta.x / 500f;
		TPLenUnit = myHP.sizeDelta.x / 100f;
		cando = true;
	}
	// Update is called once per frame
	void Update ()
	{
		if (cando) {
			myHP.sizeDelta = new Vector2 (HPLenUnit * pcon.GetHP (), myHP.sizeDelta.y);
			myTP.sizeDelta = new Vector2 (TPLenUnit * pcon.GetTP (), myTP.sizeDelta.y);
			myHPText.GetComponent<UnityEngine.UI.Text> ().text = pcon.GetHP ().ToString ();
		}
	}
}
