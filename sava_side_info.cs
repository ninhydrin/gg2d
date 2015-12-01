using UnityEngine;
using System.Collections;

public class sava_side_info : MonoBehaviour
{
	GameObject myOb;
	bool canDelete;
	bool startF;
	RectTransform rt;
	RectTransform HPrt;
	RectTransform LVrt;
	public AnimationCurve animCurve = AnimationCurve.Linear (0, 0, 1, 1);
	public Vector2 inPosition;        // スライドイン後の位置
	public Vector2 outPosition;      // スライドアウト後の位置
	public float duration = 0.3f;    // スライド時間（秒）
	float HPLenUnit;
	float LVLenUnit;

	// Use this for initialization
	void Start ()
	{
		canDelete = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		//GetComponent<UnityEngine.UI.Image> ().sprite = myOb.GetComponent<sava_base> ().face;		
		if (canDelete && myOb == null)
			Destroy (this.gameObject);
		if (startF) {
			inPosition = rt.anchoredPosition;
			HPrt.sizeDelta = new Vector2 (HPLenUnit * myOb.GetComponent<Sava_controler> ().HP, HPrt.sizeDelta.y);
			LVrt.sizeDelta = new Vector2 (LVLenUnit * myOb.GetComponent<Sava_controler> ().LV, LVrt.sizeDelta.y);
			if (Input.GetKeyDown (KeyCode.H)) {
				SlideIn ();
			}
		}
	}

	public void init (Sprite face,GameObject pre,int mHP)
	{
		myOb = pre;
		canDelete = true;
		GetComponent<UnityEngine.UI.Image> ().sprite = face;
		rt = GetComponent<RectTransform> ();
		HPrt = transform.FindChild ("Sava_side_HP").GetComponent<RectTransform> ();
		LVrt = transform.FindChild ("Sava_side_LV").GetComponent<RectTransform> ();
		HPLenUnit = HPrt.sizeDelta.x / (float)mHP;
		LVLenUnit = LVrt.sizeDelta.x / 99f;
		LVrt.sizeDelta = new Vector2 (0, 0);
	}
	public void SetOb(GameObject ob){
		myOb = ob;
		startF = true;
	}


	public void SetPos (float x, float y)
	{
		rt.anchoredPosition = new Vector2 (x, y);
	}

	
	// スライドイン（Pauseボタンが押されたときに、これを呼ぶ）
	public void SlideIn ()
	{
		StartCoroutine (StartSlidePanel (true));
	}
	
	// スライドアウト
	public void SlideOut ()
	{
		StartCoroutine (StartSlidePanel (false));
	}
	
	private IEnumerator StartSlidePanel (bool isSlideIn)
	{
		RectTransform rt = transform.GetComponent<RectTransform> ();
		float startTime = Time.time;    // 開始時間
		Vector2 startPos = rt.anchoredPosition;  // 開始位置
		Vector2 moveDistance;            // 移動距離および方向
		
		if (isSlideIn) {
			moveDistance = (inPosition - startPos);
		} else {
			moveDistance = (outPosition - startPos);
		}
		while ((Time.time - startTime) < duration) {
			rt.anchoredPosition = startPos + moveDistance * animCurve.Evaluate ((Time.time - startTime) / duration);
			yield return 0;        // 1フレーム後、再開
		}
		rt.anchoredPosition = startPos + moveDistance;
	}

}
