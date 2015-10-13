using UnityEngine;
using System.Collections;

public class Sava_side_manager : MonoBehaviour
{
	int maxSavaNum;
	Transform[] savaList;
	// Use this for initialization
	void Start ()
	{
		maxSavaNum = 19;
		savaList = new Transform[maxSavaNum];
	}
	
	// Update is called once per frame
	void Update ()
	{
		int i = 0;
		int savaNum = transform.childCount;
		foreach (Transform child in transform) {
			savaList [i] = child;
			i++;			
		}
		float x = 0f;
		float y = 0f;
		for (i=0; i<savaNum; i++) { //  ru-pu tyuu ni sava ga heru kamo
			savaList [i].GetComponent<sava_side_info> ().SetPos (x, y);
			y -= 20f;
		}
		//print (savaList [0].position);
	}

}
