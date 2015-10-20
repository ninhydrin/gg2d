using UnityEngine;
using System.Collections;

public class Basic_info : MonoBehaviour
{	
	float x, y;
	GameObject MY_MG_HP;
	GameObject MY_FACE;
	GameObject MY_MANA;
	GameObject TIME;
	int player_num;
	Color[] player_color;
	GameObject SAVA_SIDE;
	// Use this for initialization
	void Start ()
	{
		player_color = new Color[4];
		player_color [0] = Color.red;
		x = Screen.width;
		y = Screen.height;
		MY_MG_HP = transform.FindChild ("Player_MG_HP").gameObject;
		MY_MANA = MY_MG_HP.transform.FindChild ("Mana").gameObject;
		SAVA_SIDE = transform.FindChild ("Sava_side").gameObject;
		TIME = transform.FindChild ("Time").gameObject;
		TIME.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (0, -y / 9);
		MY_FACE = transform.FindChild ("Player_image").gameObject;
		MY_MG_HP.transform.FindChild("MG_HP_bar").GetComponent<UnityEngine.UI.Image> ().color = player_color [0];
		MY_MG_HP.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-x/64 * 17, -y / 16);
		SAVA_SIDE.GetComponent<RectTransform> ().anchoredPosition = new Vector2 (-x / 64*29, -y / 36*7);
	}
	
	// Update is called once per frame
	void Update ()
	{
		x = Screen.width;
		y = Screen.height;
	}
}
