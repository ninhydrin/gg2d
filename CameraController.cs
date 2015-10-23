using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
	public GameObject player;
	private Vector3 offset;
	private Vector3  lockOn;
	public float x, y, z, xx, yy, zz;
	public Button buttons;
	PlayerController playerC;
	Transform Fc;
	Transform CP;
	GameObject[] nearEnemy;
	Transform BDPos;

	bool pTarget;
	// Use this for initialization
	void Start ()
	{
		
		buttons = GameObject.Find ("Player_info").GetComponent<Button> ();
		x = 8f;
		y = 6f;
		z = -7f;
		xx = 0f;
		yy = 4f;
		zz = -5f;
		offset = new Vector3 (0f, y, z);
		lockOn = new Vector3 (0f, yy, zz);
		//transform.localRotation.x = ;
		player = GameObject.FindWithTag ("Player");
		playerC = player.GetComponent<PlayerController> ();
		CP = GameObject.Find ("Player/Camera_Pos").transform;
		BDPos = player.transform.FindChild ("BD_Pos");
		a = player.transform.position;			
		nearEnemy = new GameObject[100];

	}

	Vector3 a, b;
	int targetNum, maxTnum;
	float thita=0;
	public float ang;
	// Update is called once per frame
	void LateUpdate ()
	{
	
		b = CP.transform.position;

	
		if (playerC.targeting && !pTarget) {
			HashSet<GameObject> nowHash = playerC.nearEnemy;
			pTarget = true;
			nowHash.CopyTo (nearEnemy);
			targetNum = 0;
			maxTnum = nowHash.Count;
		}

		if (playerC.targeting) {
			if (Input.GetKey (buttons.RStick_Right)) {
				targetNum = targetNum == maxTnum-1 ? 0:	targetNum+1;					
			} else if (Input.GetKey (buttons.RStick_Left)) {
				targetNum = targetNum == 0 ? maxTnum+1:	targetNum-1;
			}
			a = nearEnemy [targetNum].transform.position;			
			player.transform.LookAt(a);
			a = new Vector3 (a.x, 0, a.z);
			a=BDPos.position;

			transform.LookAt(player.transform.position);
			
			//offset = new Vector3 (0f, y, z);
				transform.position = a;

		} else {
			if (Input.GetKey (buttons.RStick_Right)) {
				Vector3 bbb = transform.eulerAngles;
				transform.eulerAngles -= bbb;
				transform.Rotate(Vector3.up,ang);				
				thita+= 1;
				offset = new Vector3(Mathf.Sin(Mathf.PI/180*thita)*z,y,Mathf.Cos(Mathf.PI/360*thita)*z);
				transform.eulerAngles += bbb;
			} else if (Input.GetKey (buttons.RStick_Left)) {
				Vector3 bbb = transform.eulerAngles;
				transform.eulerAngles -= bbb;
				transform.Rotate(Vector3.up);				
				transform.eulerAngles += bbb;
			}
			a = player.transform.position;			
			a = new Vector3 (a.x, 0, a.z);

			//offset = new Vector3 (0f, y, z);
			transform.position = a + offset;		
		}
	
	}

}
