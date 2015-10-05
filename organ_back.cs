using UnityEngine;
using System.Collections;

public class organ_back : MonoBehaviour
{
	RectTransform backRt;
	RectTransform mapRt;
	Vector2 windowSize;
	float offset;
	cursor_controller cusorC;
	// Use this for initialization
	void Start ()
	{
		backRt = transform.GetComponent<RectTransform> ();
		mapRt = transform.FindChild ("Map").GetComponent<RectTransform> ();
		offset = 150;
		windowSize = new Vector2 (Screen.width, Screen.height);
		cusorC = transform.FindChild ("Map/Organ_Cursor").GetComponent<cursor_controller> ();
		cusorC.SetSize (offset);
	}
	
	// Update is called once per frame
	void Update ()
	{
		backRt.sizeDelta = new Vector2 (Screen.width, Screen.height);
	}
	public void SetSize(float a){
		if (offset + a < 150) {
			offset = 150;
		} else if (offset + a > 250) {
			offset = 250;
		} else {
			offset+=a;
		}
		cusorC.SetSize (offset);
	}
}
