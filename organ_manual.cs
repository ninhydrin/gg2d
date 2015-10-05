using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class organ_manual : MonoBehaviour
{
	Text[] manual_text;
	Button buttonC;
	// Use this for initialization
	void Start ()
	{
		buttonC = GameObject.Find ("Player_info").GetComponent<Button> ();
		manual_text = new Text[4];
		int i = 0;
		foreach (Transform child in transform) {
			manual_text [i] = child.GetComponent<Text> ();
			i++;
		}

	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
