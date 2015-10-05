using UnityEngine;
using System.Collections;

public class mini_master : MonoBehaviour
{
	
	public RectTransform rt;
	private GameObject player;
	private Vector2 offset;

	// Use this for initialization
	void Start ()
	{
		rt = GetComponent<RectTransform> ();
		player = GameObject.FindWithTag ("Player");
		offset = rt.anchoredPosition;
		StartCoroutine (ImSelected ());
	}
	
	// Update is called once per frame
	void Update ()
	{
		offset.x = (player.transform.position.x - 250f) / 5f;
		offset.y = (player.transform.position.z - 250f) / 5f;
		rt.eulerAngles=new Vector3 (rt.eulerAngles.x,rt.eulerAngles.y,-player.transform.eulerAngles.y);
		

		if (offset.x >= 50 || offset.y >= 50 || offset.x <= -50 || offset.y <= -50) {
			enabled = false;			
		} else {
			enabled = true;
		}

		
		rt.anchoredPosition = offset;	
	}

	private IEnumerator ImSelected ()
	{
		Vector2 or = rt.sizeDelta;

		float maxsize = 1.5f;
		float num = 0.2f;
		while (true) {
			if (rt.sizeDelta.x - or.x > maxsize)
				num = -0.2f;
			else if (rt.sizeDelta.x - or.x < -maxsize)
				num = 0.2f;
			rt.sizeDelta = new Vector2 (rt.sizeDelta.x + num, rt.sizeDelta.y + num);
			yield return 0;        // 1フレーム後、再開
		}
		
		transform.GetComponent<RectTransform> ().sizeDelta = or;
		
	}
}
