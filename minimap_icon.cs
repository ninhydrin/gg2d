using UnityEngine;
using System.Collections;

public class minimap_icon : MonoBehaviour {

	// Use this for initialization
	public RectTransform rt;
	GameObject myOb;
	private Vector2 offset;
	float inter;
	// Use this for initialization
	void Start ()
	{
		rt = GetComponent<RectTransform> ();

	}
	
	// Update is called once per frame
	void Update ()
	{

		if (myOb == null) {
			Destroy (this.gameObject);
		} else {
		
			offset.x = (myOb.transform.position.x - 250f) / 5f;
			offset.y = (myOb.transform.position.z - 250f) / 5f;
			rt.anchoredPosition = offset;
		}


	}
	
	public void init (GameObject nicon)
	{
		myOb = nicon;
		rt = GetComponent<RectTransform> ();
		offset.x = (myOb.transform.position.x - 250f) / 5f;
		offset.y = (myOb.transform.position.z - 250f) / 5f;
		rt.anchoredPosition = offset;
	}

}
