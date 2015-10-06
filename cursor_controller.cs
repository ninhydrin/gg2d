using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class cursor_controller : MonoBehaviour
{
	public RectTransform rt;
	private GameObject player;
	private Vector2 offset;
	public int cursor_speed = 11;
	float tox;
	float toy;
	float forcef;
	int targetG;
	public GameObject targetOb;
	float moveH;
	float moveV;
	public float mapSize;

	organ_controller organC;
	private Image MBdest;
	private Text MBdestT;
	private Image MBcancel;
	private Text MBcancelT;
	private Image MBmarch;
	private Text MBmarchT;
	private Image MBmarchI;
	private Image MBisMarch;
	private Text MBisMarchT;
	private Image MBisMarchI;
	private Image MBmove;
	private Text MBmoveT;
	private Image MBmoveI;
	private Image MBisMove;
	private Text MBisMoveT;
	private Image MBisMoveI;
	private Image MBgroup;
	private Text MBgroupT;
	private Image MBgroupI;
	private Image MBisgroup;
	private Text MBisgroupT;
	private Image MBisgroupI;



	// Use this for initialization
	
	void Start ()
	{
		rt = GetComponent<RectTransform> ();
		player = GameObject.FindWithTag ("Player");
		organC = player.GetComponent<organ_controller> ();
		MBdestT = transform.FindChild ("Message_dest/Text").GetComponent<Text> ();
		MBdest = transform.FindChild ("Message_dest").GetComponent<Image> ();
		MBcancel = transform.FindChild ("Message_cancel").GetComponent<Image> ();
		MBcancelT = transform.FindChild ("Message_cancel/Text").GetComponent<Text> ();
		MBmarch = transform.FindChild ("Message_marching").GetComponent<Image> ();
		MBmarchT = transform.FindChild ("Message_marching/Text").GetComponent<Text> ();
		MBmarchI = transform.FindChild ("Message_marching/Image").GetComponent<Image> ();
		MBisMarch = transform.FindChild ("Message_isMarching").GetComponent<Image> ();
		MBisMarchT = transform.FindChild ("Message_isMarching/Text").GetComponent<Text> ();
		MBisMarchI = transform.FindChild ("Message_isMarching/Image").GetComponent<Image> ();
		MBmove = transform.FindChild ("Message_move").GetComponent<Image> ();
		MBmoveT = transform.FindChild ("Message_move/Text").GetComponent<Text> ();
		MBmoveI = transform.FindChild ("Message_move/Image").GetComponent<Image> ();
		MBisMove = transform.FindChild ("Message_isMove").GetComponent<Image> ();
		MBisMoveT = transform.FindChild ("Message_isMove/Text").GetComponent<Text> ();
		MBisMoveI = transform.FindChild ("Message_isMove/Image").GetComponent<Image> ();
		MBgroup = transform.FindChild ("Message_grouping").GetComponent<Image> ();
		MBgroupT = transform.FindChild ("Message_grouping/Text").GetComponent<Text> ();
		MBgroupI = transform.FindChild ("Message_grouping/Image").GetComponent<Image> ();
		MBisgroup = transform.FindChild ("Message_isGrouping").GetComponent<Image> ();
		MBisgroupT = transform.FindChild ("Message_isGrouping/Text").GetComponent<Text> ();
		MBisgroupI = transform.FindChild ("Message_isGrouping/Image").GetComponent<Image> ();

		mapSize = 150;	

		offset = rt.anchoredPosition;
		tox = 0f;
		toy = 0f;
		forcef = 1.5f;
		moveH = 0f;
		moveV = 0f;
		targetG = 0;
	}
	// Update is called once per frame
	void Update ()
	{
	
		if (player.GetComponent<PlayerController> ().organ) {		
			if (!organC.ismenu || organC.summoning) {
								
				moveH = Input.GetAxis ("Horizontal");
				moveV = Input.GetAxis ("Vertical");
				moveH = offset.x + moveH * cursor_speed - tox;
				moveV = offset.y + moveV * cursor_speed - toy;

				if (moveH > mapSize) {
					moveH = mapSize;
				} else if (moveH < -mapSize) {
					moveH = -mapSize;
				}
				if (moveV > mapSize) {
					moveV = mapSize;
				} else if (moveV < -mapSize) {
					moveV = -mapSize;
				}
				
				offset = new Vector2 (moveH, moveV);
				rt.anchoredPosition = offset;
				tox = 0f;
				toy = 0f;


			}						
		} else {
			offset = rt.anchoredPosition = new Vector2 (0, 0);
			targetG = 0;
		}
		

	}

	public Vector2 LocalPos ()
	{
		return rt.anchoredPosition;
	}

	public Vector2 RealPos ()
	{
		return new Vector2 (rt.anchoredPosition.x * 5 / 3 + 250, rt.anchoredPosition.y * 5 / 3 + 250);
	}

	public void ForceTo (float x, float y)
	{

		if (Mathf.Abs (x) < forcef) {
			tox = 0;
		} else if (x < 0) {
			tox = -0.5f; //forcef;
		} else {
			tox = 0.5f;//forcef;
		}

		if (Mathf.Abs (y) < forcef) {
			toy = 0;
		} else if (y < 0) {
			toy = -0.5f;//forcef;
		} else {
			toy = 0.5f;//forcef;
		}
	}

	public void MoveTo (Vector2 point, int num, GameObject ptargetOb)
	{
		targetOb = ptargetOb;
		targetG = num;
		offset = point;
		moveH = 0f;
		moveV = 0f;
		//rt.anchoredPosition = point;
	}

	public void TargetReset ()
	{
		targetG = 0;
	}

	public int isTarget ()
	{
		return targetG;
	}

	public bool isMoving ()
	{
		if (Input.GetAxis ("Horizontal") != 0 ||
			Input.GetAxis ("Vertical") != 0)
			return true;
		else
			return false;
	}

	public void TargetingNum (int a)
	{
		targetG = a;
	}

	public void SetMBisMove (bool a)
	{
		MBmove.enabled = a;
		MBmoveI.enabled = a;
		MBmoveT.enabled = a;
		
	}

	public void SetMBisMarch (bool a)
	{
		MBisMarch.enabled = a;
		MBisMarchI.enabled = a;
		MBisMarchT.enabled = a;
	}

	public void SetMBisGroup (bool a)
	{
		MBisgroup.enabled = a;
		MBisgroupI.enabled = a;
		MBisgroupT.enabled = a;
	}

	public void SetMBdest (bool a)
	{
		MBdest.enabled = a;
		MBdestT.enabled = a;
	}
	bool kkk;
	public void SetMBcancel (bool a)
	{
		
		kkk = true;
		MBcancel.enabled = a;
		MBcancelT.enabled = a;
		StartCoroutine (FadeOut());
	}

	public void SetMBmove (bool a)
	{
		MBmove.color += new Color (0, 0, 0, 1f);
		MBmove.enabled = a;
		MBmoveT.enabled = a;
		MBmoveI.enabled = a;
		StopCoroutine ("FadeOut");
		StartCoroutine (FadeOutT (MBmove, MBmoveT, MBmoveI));
		
	}

	public void SetMBgroup (bool a)
	{
		MBgroup.enabled = a;
		MBgroupT.enabled = a;
		MBgroupI.enabled = a;		
		StartCoroutine (FadeOutT (MBgroup, MBgroupT, MBgroupI));
		
	}

	public void SetMBmarch (bool a)
	{
		MBmarch.enabled = a;
		MBmarchT.enabled = a;
		MBmarchI.enabled = a;
		StartCoroutine (FadeOutT (MBmarch, MBmarchT, MBmarchI));
		
	}

	public IEnumerator FadeOut ()
	{
		kkk = false;
		Color d = new Color (MBcancel.color.r, MBcancel.color.g, MBcancel.color.b, 1f);
		Color e = new Color (MBcancelT.color.r, MBcancelT.color.g, MBcancelT.color.b, 1f);
		int f = 0;
		while (f<80) {
			if(kkk){
				print ("break");
				MBcancel.enabled = false;
				MBcancelT.enabled = false;
				kkk=false;
				yield break;
			}
			MBcancel.color += new Color (0, 0, 0, -0.02f);
			MBcancelT.color += new Color (0, 0, 0, -0.02f);
			f++;
			yield return 10;        // 1フレーム後、再開
		}
		print ("end");
		MBcancel.color = d;
		MBcancelT.color = e;
		MBcancel.enabled = false;
		MBcancelT.enabled = false;
	}

	public IEnumerator FadeOutT (Image a, Text b, Image c)
	{
		Color d = a.color;
		Color e = b.color;
		Color g = c.color;
		int f = 0;
		while (f<80) {
			a.color += new Color (0, 0, 0, -0.02f);
			b.color += new Color (0, 0, 0, -0.02f);
			c.color += new Color (0, 0, 0, -0.02f);
			f++;
			yield return 10;        // 1フレーム後、再開
		}
		a.color = d;
		b.color = e;
		c.color = g;
		a.enabled = false;
		b.enabled = false;
		c.enabled = false;
	}

	public void SetSize (float a)
	{

		mapSize = a;
	}
}