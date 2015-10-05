using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ghost_icon : MonoBehaviour
{
	int myNum;
	RectTransform rt;
	ghost_base ghostB;
	// Use this for initialization
	void Start ()
	{
		rt = transform.GetComponent<RectTransform> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (ghostB.dominator > 0) {
			GetComponent<Image>().color = ghostB.domiColor;
		}
	}

	public void init (int num, Vector2 offset,ghost_base a)
	{
		rt = transform.GetComponent<RectTransform> ();
		myNum = num;
		transform.FindChild ("Number").GetComponent<UnityEngine.UI.Text> ().text = num.ToString ();
		rt.anchoredPosition = offset;
		ghostB = a;
	}
}
