
using UnityEngine;
using System.Collections;
	
// 3Dキャラの頭上にCanvas上のテキストを表示（宴本p78)
	
public class player_name : MonoBehaviour
{
		
	public Transform target;
	private Vector3 screenPoint;
	private Vector3 realPoint;
	
	void LateUpdate ()
	{
		
		screenPoint = Camera.main.WorldToScreenPoint (target.position);
		realPoint = target.position;		
		transform.position = new Vector3 (screenPoint.x, screenPoint.y, 0);
	}
}

