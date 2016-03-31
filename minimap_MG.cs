using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class minimap_MG : Photon.MonoBehaviour {

	For_next forNext;
	int myNum;
	GameObject targetMG;
	GameInfo gameInfo;
	// Use this for initialization
	void Start () {
		forNext = GameObject.Find ("ForNextScene").GetComponent<For_next> ();
		gameInfo = GameObject.Find ("GameMaster").GetComponent<GameInfo> ();
		myNum = forNext.ownerIdToNum [photonView.ownerId];	    
		targetMG = GameObject.FindWithTag(myNum.ToString()+ "P_MG");
		transform.SetParent (GameObject.Find ("Minimap/Field").transform);		
		float x = (targetMG.transform.position.x - 250f) / 5f;
		float y = (targetMG.transform.position.z - 250f) / 5f;

		GetComponent<Image> ().color = gameInfo.myColor;
		GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
		GetComponent<RectTransform> ().localScale = new Vector3 (1f, 1f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		float x = (targetMG.transform.position.x - 250f) / 5f;
		float y = (targetMG.transform.position.z - 250f) / 5f;

		GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
	
	}
}
