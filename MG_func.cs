using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MG_func : MonoBehaviour
{

	Queue<GameObject> sava_queue;
	private GameObject player;
	public RectTransform rt;
	GameObject sava;
	bool creating;
	Vector3[] summonPos;
	GameObject[] summonGate;
	// Use this for initialization
	void Start ()
	{
		player = GameObject.FindWithTag ("Player");		
		rt = GetComponent<RectTransform> ();
		sava_queue = new Queue<GameObject> (){};
		creating = false;
		sava = null;

		float x = (transform.position.x - 250f) / 5f;
		float y = (transform.position.z - 250f) / 5f;
		GameObject.Find ("MyMG").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
		summonPos = new Vector3[6];
		summonPos [0] = new Vector3 (35f, 5f, 50f);
		summonPos [1] = new Vector3 (40f, 5f, 50f);
		summonPos [2] = new Vector3 (45f, 5f, 50f);
		summonPos [3] = new Vector3 (50f, 5f, 45f);
		summonPos [4] = new Vector3 (50f, 5f, 40f);
		summonPos [5] = new Vector3 (50f, 5f, 35f);
		summonGate = new GameObject[6];
	}
	
	// Update is called once per frame
	void Update ()
	{
	
		if (sava_queue.Count > 0 && !creating) {
			creating = true;
			sava = sava_queue.Dequeue ();
			Invoke ("make", sava.GetComponent<sava_base> ().summonTime);

		}
		
		if (sava != null) {

		}

		//	GetComponent<UnityEngine.UI.Image>().enabled=true;			

	}

	void make ()
	{		
		sava.GetComponent<sava_base> ().create ();
		creating = false;

	}

	public void Order (GameObject ob)
	{
		sava_queue.Enqueue (ob);

	}

}
