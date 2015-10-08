using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class item : MonoBehaviour
{
	public string itemName;
	public int useNum;
	public int sellMoney;
	public Sprite icon;
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void UseMe (int a, GameObject player)
	{
		PlayerController playerC=player.GetComponent<PlayerController>();
		useNum--;
		if (itemName == "LPL") {
			playerC.AddHP(300);
			HashSet<GameObject> neighborhoodSava = playerC.GetNeighborhood (player.transform.position, 9);
			foreach (GameObject nSava in neighborhoodSava) {
				nSava.transform.parent.GetComponent<sava_base> ().Repair (300);
			}
		} else if (itemName == "TPL") {
			playerC.AddTP(75);
		}
		if (useNum <= 0) {
			DestroyImmediate (gameObject);
			playerC.SetItemSprite (a, null);
		}
	}

}
