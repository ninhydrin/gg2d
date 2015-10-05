using UnityEngine;
using System.Collections;

public class organ : MonoBehaviour
{
	bool organState;
	bool menuState;

	void Start ()
	{
		organState = false;
		menuState = false;

	}
	
	// Update is called once per frame
	void Update ()
	{
		if (organState) {
			GetComponent<Canvas> ().enabled = true;
		} else {
			GetComponent<Canvas> ().enabled = false;
		}
		if (menuState) {
			transform.FindChild ("Menu").GetComponent<Canvas> ().enabled = true;
		} else {
			transform.FindChild ("Menu").GetComponent<Canvas> ().enabled = false;
		}
		

	}

	public	bool isOrgan ()
	{
		return organState;
	}

	public bool isMenu ()
	{
		return menuState;
	}

	public void setOrgan ()
	{
		organState = true;
	}

	public void unsetOrgan ()
	{
		organState = false;
		menuState = false;
	}

	public void setMenu ()
	{
		menuState = true;
	}

	public void unsetMenu ()
	{
		menuState =false;
	}
}
