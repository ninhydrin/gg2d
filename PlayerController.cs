using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public Terrain terra;
	public  bool organ;
	public  bool menu;
	private GameObject MG;
	public Vector3 acceleration = new Vector3 (0, -20f, 0);	// 加速度
	public Button buttons;
	GameObject[] itemBox;
	Image[] itemSlot;
	RectTransform itemSelector;
	int itemPoint;
	int maxHP = 500;
	int nowHP;
	int maxTP = 100;
	int nowTP;

	void Start ()
	{
		organ = false;
		GameObject.Find ("Organ").GetComponent<UnityEngine.Canvas> ().enabled = false;
		MG = GameObject.FindWithTag ("PlayerMG");
		buttons = GameObject.Find ("Player_info").GetComponent<Button> ();
		nowHP = maxHP;
		nowTP = maxTP / 2;
		itemSelector = GameObject.Find ("Player_info/Item_slot/Item_selector").GetComponent<RectTransform> ();
		itemSlot = new Image[6];
		for (int i =0; i<6; i++)
			itemSlot [i] = GameObject.Find ("Player_info/Item_slot/slot" + i.ToString ()).GetComponent<Image>();
		itemBox = new GameObject[6];
		itemPoint = 0;
	}
	
	Collider[] neighborhoodSava;
	
	void Update ()
	{
		float moveH = Input.GetAxis ("Horizontal");
		float moveV = Input.GetAxis ("Vertical");
		float moveU = 0.0f;
		float sle;
		
		
		if (!organ) {

			sle = transform.position [1] - Terrain.activeTerrain.SampleHeight (transform.position);

			if (Input.GetButtonDown ("Jump") && sle < 0.6) {
				moveU = 100.0f;
			}
	
//			Vector3 movement = new Vector3 (moveH, moveU, moveV);

//			GetComponent<Rigidbody> ().AddForce (movement * speed * Time.deltaTime);

			if (Input.GetKeyDown (buttons.Pad_Left)) {
				organ = true;
			}
			if (Input.GetKeyDown (buttons.B_Button)) {			
				UseItem(itemPoint);
			}

			if (Input.GetKeyDown (buttons.RB)) {
				itemPoint = (itemPoint > 4) ? 0 : itemPoint + 1;
				MoveItemSelector ();
			} else if (Input.GetKeyDown (buttons.LB)) {			
				itemPoint = (itemPoint < 1) ? 5 : itemPoint - 1;
				MoveItemSelector ();				
			}				






		} else {

			if (Input.GetKeyDown (buttons.Pad_Left)) {
				//organ = false;
			}
		} 

		if (Input.GetKeyDown (KeyCode.E)) {
		
		}

	}

	void MoveItemSelector ()
	{
		itemSelector.anchoredPosition = new Vector3 (4 + itemPoint + itemPoint * 22, itemSelector.anchoredPosition.y);
	}

	void UseItem (int a)
	{
		if (itemBox [a] != null) {
			if(itemBox[a].GetComponent<item>()!=null){
				print ("use");
				itemBox[a].GetComponent<item>().UseMe(a,gameObject);
			}
		
		
		}

	}

	public int GetHP ()
	{
		return nowHP;
	}

	public int GetTP ()
	{
		return nowTP;
	}




	public HashSet<GameObject> GetNeighborhood (Vector3 pos, float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (pos, range);
		foreach (Collider b in a) {
			if (b.gameObject.tag == "My_sava")
				c.Add (b.gameObject);
		}
		return c;
	}

	public int IsEmp ()
	{
		for(int i = 0;i<6;i++){
			if(itemBox[i]==null){
				return i;
			}
		}	
		return -1;
	}
	public void AddItem (int a,GameObject b){
		itemBox [a] = b;
	}
	public void SetItemSprite(int a,Sprite b){
		itemSlot[a].sprite = b;
	}
	public void Damage (int num)
	{
		if (nowHP - num < 0)
			nowHP = 0;
		else
			nowHP -= num;		
	}
}
