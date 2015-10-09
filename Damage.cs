using UnityEngine;
using System.Collections;

public class Damage : MonoBehaviour {

	int time=0;
	Vector3 pos;
	float num = 0.5f;
	// Update is called once per frame
	void Update () {
		time++;
		Vector3 k = Camera.main.WorldToScreenPoint (pos);
		num += 0.5f;
		transform.position = new Vector3(k.x,k.y+num,0f)	;
	if (time > 50)
			DestroyImmediate (gameObject);
	}
	public void DamageInt(int a,GameObject b,Vector3 c){
		transform.GetComponent<UnityEngine.UI.Text>().text=a.ToString();
		transform.parent=b.transform;
		pos = c;
		pos.y += 2f;
	}
}
