using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Damage : MonoBehaviour {

	int time=0;
	Vector3 pos;
	float num = 0.5f;
	// Update is called once per frame
	void Update () {
		time++;
		Vector3 k = Camera.main.WorldToScreenPoint (pos);
		num += 2f;
		transform.position = new Vector3(k.x,k.y+num,0f)	;
	if (time > 50)
			DestroyImmediate (gameObject);
	}
	public void DamageInt(int a,GameObject b,Vector3 c){
		Text me = transform.GetComponent<Text> ();
		me.text=a.ToString();
		if (a > 200) {
			me.fontSize = 50;
		} else if (a > 150) {
			me.fontSize=40;
		}
		else if (a >= 100) {
			me.fontSize = 30;
		}
		
		transform.parent=b.transform;
		pos = c;
		pos.y += 2f;
	}
}
