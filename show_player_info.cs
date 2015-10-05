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
	// Use this for initialization
	void Start ()
	{
		myPlayer = GameObject.FindWithTag ("Player");
		pcon = myPlayer.GetComponent<PlayerController> ();
		myHP = transform.FindChild ("Player_HP").GetComponent<RectTransform> ();
		myTP = transform.FindChild ("Player_TP").GetComponent<RectTransform> ();
		myHPText = transform.FindChild ("Player_HP_Text");
		HPLenUnit = myHP.sizeDelta.x / 500f;
		TPLenUnit = myHP.sizeDelta.x / 100f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		myHP.sizeDelta=new Vector2 (HPLenUnit * pcon.GetHP (),myHP.sizeDelta.y);
		myTP.sizeDelta=new Vector2 (TPLenUnit * pcon.GetTP (),myTP.sizeDelta.y);
		myHPText.GetComponent<UnityEngine.UI.Text>().text=pcon.GetHP().ToString();
	}
}
