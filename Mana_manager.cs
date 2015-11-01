using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Mana_manager : MonoBehaviour
{
	int mana;
	int income;
	bool flag;
	Text TextMana;
	Text TextIncom;
	Game_All_Init GameMaster;
	int Mynum;
	// Use this for initialization
	void Start ()
	{
		mana = 6000;
		income = 20;
		flag = false;
		TextMana = transform.FindChild ("Possession").GetComponent<UnityEngine.UI.Text> ();
		TextIncom = transform.FindChild ("Income").GetComponent<UnityEngine.UI.Text> ();
		TextMana.text="600mana";
		TextIncom.text=income.ToString()+ "mana";
		GameMaster = GameObject.Find ("GameMaster").GetComponent<Game_All_Init> ();

	}
	
	// Update is called once per frame
	void Update ()
	{
		int a = (int)GameObject.FindWithTag ("Time").GetComponent<Timelimit> ().RemainingTime;

		int b = 1;//int b=GameMaster.GetMyGhost (1);
		income = 20 * (b + 1);
		if (a % 10 == 0 && flag) {
			mana += income;
			flag = false;
		}
		if (a % 10 == 1) {
			flag = true;
		}
		TextMana.text = mana.ToString () + "mana";
		TextIncom.text = "+" + income.ToString ();

	}

	void SetIncome ()
	{
		income = 3;
	}

	public  int GetMana ()
	{
		return mana;
	}

	public void AddMana (int num)
	{
		mana += num;
	}

	public void RemoveMana (int num)
	{
		mana -= num;
	}

}
