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
	Transform DefaultPos;
	Transform LOP;
	GameObject[] nearEnemy;
	Transform BDPos;
	bool pTarget;
	float turnTime;
	// Use this for initialization
	void Start ()
	{
		
		buttons = GameObject.Find ("Player_info").GetComponent<Button> ();
		x = 8f;
		y = 6f;
		z = -8f;
		xx = 0f;
		yy = 4f;
		zz = -5f;
		offset = new Vector3 (0f, y, z);
		lockOn = new Vector3 (0f, yy, zz);
		//transform.localRotation.x = ;
		player = GameObject.FindWithTag ("Player");
		playerC = player.GetComponent<PlayerController> ();
		CP = GameObject.Find ("Player/Camera_Pos").transform;
		LOP = player.transform.FindChild ("LockOnPos");
		DefaultPos = player.transform.FindChild ("DefaultPos");
		BDPos = player.transform.FindChild ("BD_Pos");
		a = player.transform.position;			
		nearEnemy = new GameObject[100];
		transform.position = a + offset;
		
	}

	Vector3 a, b;
	int targetNum, maxTnum;
	float thita = 0;
	public float ang;
	int cameraFlag;
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
			cameraFlag = 5;
			if (Input.GetKey (buttons.RStick_Right)) {
				targetNum = targetNum == maxTnum - 1 ? 0 : targetNum + 1;					
			} else if (Input.GetKey (buttons.RStick_Left)) {
				targetNum = targetNum == 0 ? maxTnum + 1 : targetNum - 1;
			}
			a = nearEnemy [targetNum].transform.position;			
			player.transform.LookAt (a);
			transform.LookAt (a);			
			a = LOP.position;
			transform.position = Vector3.Lerp (transform.position, a, 4f * Time.deltaTime);

			a = new Vector3 (a.x, 0f, a.z);
			b = new Vector3 (player.transform.position.x, 0f, player.transform.position.z);
			offset = (b - a).normalized * z + Vector3.up * y;
			thita = Mathf.Asin(offset.x /z)/Mathf.PI*180;
			
			
			//offset = new Vector3 (0f, y, z);
		
		} else {
			a = new Vector3 (player.transform.position.x, 0f, player.transform.position.z);
			b = new Vector3 (transform.position.x, 0f, transform.position.z);
			if (Input.GetKey (buttons.RStick_Right)) {
				Vector3 bbb = transform.eulerAngles;
				thita += 2;
				transform.eulerAngles -= bbb;
				offset = new Vector3 (Mathf.Sin (Mathf.PI / 180 * thita) * z, y, Mathf.Cos (Mathf.PI / 180 * thita) * z);
				transform.eulerAngles += bbb;
			} else if (Input.GetKey (buttons.RStick_Left)) {
				Vector3 bbb = transform.eulerAngles;
				thita -= 2;
				transform.eulerAngles -= bbb;
				offset = new Vector3 (Mathf.Sin (Mathf.PI / 180 * thita) * z, y, Mathf.Cos (Mathf.PI / 180 * thita) * z);
				transform.eulerAngles += bbb;
			}
					
			a = player.transform.position;			
			a = new Vector3 (a.x, 0, a.z);
			if(Input.GetAxis ("Horizontal")>0 || Input.GetAxis ("Vertical") > 0){
				turnTime = 8f;
				transform.position=a+offset;
			}else {
				turnTime=4f;
			transform.position = Vector3.Lerp (transform.position, a + offset, turnTime * Time.deltaTime);
				
			}
//			transform.position = Vector3.Lerp (transform.position, a + offset, turnTime * Time.deltaTime);
			
			//transform.LookAt(new Vector3(a.x-DefaultPos.position.x,3f,a.z-DefaultPos.position.z));
			//print(2 * a - new Vector3 (transform.position.x, 0, transform.position.z) + Vector3.up * 3);
			transform.LookAt (2 * a - new Vector3 (transform.position.x, 0, transform.position.z) + Vector3.up * 3);
			//transform.LookAt(DefaultPos);

			//transform.position = a + offset;		
		}
	
	}

}
