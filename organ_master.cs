using UnityEngine;
using System.Collections;

public class organ_master : MonoBehaviour
{

	// Use this for initialization

	public RectTransform rt;
	private GameObject player;
	private Vector2 offset;
	public GameObject map;
	private float x,y;
	// Use this for initialization
	void Start ()
	{
		rt = GetComponent<RectTransform> ();
		player = GameObject.FindWithTag ("Player");
		offset = rt.anchoredPosition;
		x = GameObject.Find ("Map").GetComponent<RectTransform>().sizeDelta.x;
		y = GameObject.Find ("Map").GetComponent<RectTransform>().sizeDelta.y;
		
	}
		
	// Update is called once per frame
	void Update ()
	{
		offset.x = (player.transform.position.x - 250f) * 0.6f;
		offset.y = (player.transform.position.z - 250f) * 0.6f;
		rt.anchoredPosition = offset;
		if (offset.x >= x || offset.y >= y || offset.x <= -x || offset.y <= -y) {
			enabled = false;			
		} else {
			enabled = true;
		}			
	}
}