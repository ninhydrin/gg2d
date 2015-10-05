using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public Terrain terra;
	public  bool organ;
	public  bool menu;
	private GameObject MG;
	public Vector3 acceleration = new Vector3 (0, -20f, 0);	// 加速度
	public Button buttons;
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
				//Collider[] neighborhoodSava = GetNeighborhood (transform.position, 9);
				HashSet<GameObject> neighborhoodSava = GetNeighborhood (transform.position, 9);
				foreach (GameObject nSava in neighborhoodSava) {
					nSava.transform.parent.GetComponent<sava_base> ().Repair (150);
				}
			}

		} else {

			if (Input.GetKeyDown (buttons.Pad_Left)) {
				//organ = false;
			}
		} 

		if (Input.GetKeyDown (KeyCode.E)) {
		
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

	HashSet<GameObject> GetNeighborhood (Vector3 pos, float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (pos, range);
		foreach (Collider b in a) {
			if (b.gameObject.tag == "My_sava")
				c.Add (b.gameObject);
		}
		return c;
	}

	public void Damage (int num)
	{
		if (nowHP - num < 0)
			nowHP = 0;
		else
			nowHP -= num;		
	}
}
