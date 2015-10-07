using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
	public GameObject player;
	private Vector3 offset;
	private Vector3 rot;
	public float x, y, z, xx, yy, zz;
	public Button buttons;
	Transform Fc;
	Transform CP;
	// Use this for initialization
	void Start ()
	{
		buttons = GameObject.Find ("Player_info").GetComponent<Button> ();
		x = 8f;
		y = 6f;
		z = -7f;
		xx = 0f;
		yy = 4f;
		zz = 8f;
		offset = new Vector3 (0f, y, z);
		rot = new Vector3 (zz, 0f, 0f);
		//transform.localRotation.x = ;
		player = GameObject.FindWithTag ("Player");
		
		CP = GameObject.Find ("Player/Camera_Pos").transform;
		a = player.transform.position;			

	}

	Vector3 a, b;
	// Update is called once per frame
	void LateUpdate ()
	{
		if (Input.GetKey (buttons.RStick_Right)) {
			//transform.RotateAround (player.transform.position, Vector3.up, 1f);
			
		} else if (Input.GetKey (buttons.RStick_Left)) {
		}
		b = CP.transform.position;

	
		if (Input.GetKey (buttons.LB) || Input.GetKey (buttons.RB) || true) {
			a = player.transform.position;			
			a = new Vector3 (a.x, 0, a.z);
			//offset = new Vector3 (0f, y, z);
			transform.position = a + offset;
		} else {

			offset = new Vector3 (xx, yy, zz);
			if ((a - player.transform.position).magnitude > 3) {
				a = player.transform.position;
				transform.position = player.transform.position + offset;
			}
			transform.LookAt (player.transform.position);

			//transform.rotation = player.transform.rotation;
			//print (transform.rotation);

		}
	
	}
}
