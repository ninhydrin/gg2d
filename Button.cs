using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{

	public KeyCode A_Button;
	public KeyCode B_Button;
	public KeyCode X_Button;
	public KeyCode Y_Button;
	public KeyCode LT;
	public KeyCode LB;
	public KeyCode RT;
	public KeyCode RB;
	public KeyCode LStick_Button;
	public KeyCode LStick_Left;
	public KeyCode LStick_Right;
	public KeyCode LStick_Up;
	public KeyCode LStick_Down;
	public KeyCode RStick_Button;
	public KeyCode RStick_Left;
	public KeyCode RStick_Right;
	public KeyCode RStick_Up;
	public KeyCode RStick_Down;
	public KeyCode Pad_Left;
	public KeyCode Pad_Right;
	public KeyCode Pad_UP;
	public KeyCode Pad_Down;
	public KeyCode Start_B;
	public KeyCode Select_B;
	int Mana;		
	
	// Use this for initialization
	void Start ()
	{
		A_Button = KeyCode.K;
		B_Button = KeyCode.L;
		X_Button = KeyCode.J;
		Y_Button = KeyCode.I;
		LB = KeyCode.Alpha1;
		RB = KeyCode.Alpha0;
		LT = KeyCode.P;
		RT = KeyCode.T;
		LStick_Button = KeyCode.Q;
		LStick_Left = KeyCode.A;
		LStick_Right = KeyCode.D;
		LStick_Up = KeyCode.W;
		LStick_Down = KeyCode.S;
		Pad_Left = KeyCode.Z;

		Mana = 600;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public int GetMana ()
	{
		return Mana;
	}
}
