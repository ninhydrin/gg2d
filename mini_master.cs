using UnityEngine;
using System.Collections;

public class mini_master : MonoBehaviour
{
	
	public RectTransform rt;
	private GameObject player;
	private Vector2 offset;
	int myNum;
	For_next forNext;
	private bool cando;
	// Use this for initialization
	void Start ()
	{
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		myNum = forNext.myid;
		rt = GetComponent<RectTransform> ();
		offset = rt.anchoredPosition;
		StartCoroutine (WaitInit ());
	}
	
	// Update is called once per frame
	void Update ()
	{if (cando) {
			offset.x = (player.transform.position.x - 250f) / 5f;
			offset.y = (player.transform.position.z - 250f) / 5f;
			rt.eulerAngles = new Vector3 (rt.eulerAngles.x, rt.eulerAngles.y, -player.transform.eulerAngles.y);
		

			if (offset.x >= 50 || offset.y >= 50 || offset.x <= -50 || offset.y <= -50) {
				enabled = false;			
			} else {
				enabled = true;
			}					
			rt.anchoredPosition = offset;	
		}
	}

	IEnumerator WaitInit(){
		commons common = GameObject.FindWithTag ("commons").GetComponent<commons> ();
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();		
		while (!common.myOk) {
			yield return 0;
		}
		player = GameObject.FindWithTag (myNum.ToString()+"P_Master");
		StartCoroutine (ImSelected ());		
		cando = true;
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
