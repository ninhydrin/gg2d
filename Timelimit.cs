using UnityEngine;
using System.Collections;

public class Timelimit: MonoBehaviour
{

	public static class Delegate
	{
		
		public delegate void VoidDelegate ();

		public delegate void BoolDelegate (bool value);

		public delegate void FloatDelegate (float value);

		public delegate void VectorDelegate (Vector2 value);

		public delegate void StringDelegate (string value);

		public delegate void ObjectDelegate (GameObject value);

		public delegate void KeyCodeDelegate (KeyCode value);
		
		public delegate void VoidGameObjectDelegate (GameObject gameObject);

		public delegate void BoolGameObjectDelegate (GameObject gameObject,bool value);

		public delegate void FloatGameObjectDelegate (GameObject gameObject,float value);

		public delegate void VectorGameObjectDelegate (GameObject gameObject,Vector2 value);

		public delegate void StringGameObjectDelegate (GameObject gameObject,string value);

		public delegate void ObjectGameObjectDelegate (GameObject gameObject,GameObject gameObject2);

		public delegate void KeyCodeGameObjectDelegate (GameObject gameObject,KeyCode value);
	}

	public float CurrentTime {
		get;
		private set;
	}
	
	/// <summary>
	/// 残り時間
	/// </summary>
	/// <value>The remaining time.</value>
	public float RemainingTime {
		get {
			return LimitTime - CurrentTime;
		}
		private set {
		}
	}
	
	/// <summary>
	/// 停止時間
	/// </summary>
	/// <value>The limit time.</value>
	public float LimitTime = 300f;

	
	/// <summary>
	/// LimitTimeまで時間が進んだら呼ばれる
	/// </summary>
	/// <value>The fire delegate.</value>
	public Delegate.VoidDelegate FireDelegate {
		get;
		set;
	}

	public string Tomin (int nowtime)
	{
		string m = (nowtime / 60).ToString ();
		int s = (nowtime % 60);
		if (s < 10) 
			return "0" + m + ":" + "0" + s;
		else
			return "0" + m + ":" + s;


	}

	bool isEnable = true;

	public bool IsEnable {
		get {
			return isEnable;
		}
		
		set {
			isEnable = value;
			if (!value) {
				CurrentTime = 0;
			}
		}
	}
	
	/// <summary>
	/// 駆動中または有効になっていない場合はFalse、
	/// 時間に来たらTrueを返す。
	/// </summary>
	public bool Update ()
	{
		if (IsEnable) {
			CurrentTime += Time.deltaTime;
			transform.GetComponent<UnityEngine.UI.Text> ().text = Tomin ((int)RemainingTime);
			if (CurrentTime >= LimitTime) {
				CurrentTime = 0;
				if (FireDelegate != null) {
					FireDelegate ();
				}
				return true;
			}
			return false;
		} else {
			return false;
		}
	}
}
