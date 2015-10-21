using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sava_base : MonoBehaviour
{
	
	public GameObject ob;
	public GameObject headHP;
	public GameObject sideHP;
	public Sprite face;
	public Vector2 dest;
	public float speed;
	public int HP;
	public int LV;
	public int maxHP;
	public int summonTime;

	public GameObject minimap_icon_ob;
	public GameObject map_icon_ob;
	public GameObject headHP_ob;
	public GameObject sideHP_ob;
	public int myGroupNum;
	public bool isLeader;

	GameObject me;
	GameObject mySava;
	sava_report toSubmission;
	
	bool forRearch;
	bool forSearch;
	bool canReport;
	Camera mainCam;
	public GameObject damageText;
	GameObject minimap;

	// Use this for initialization
	void Start ()
	{
		mySava = GameObject.Find ("Organ/Map/Organ_sava");
		face = Resources.Load<Sprite> ("query-chan");
		toSubmission = GameObject.Find ("Player_info/Sava_report").GetComponent<sava_report> ();
		me = null;
		canReport = true;
		mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		minimap = GameObject.Find("Minimap");
	}
	
	// Update is called once per frame
	void Update ()
	{
		/*
		if (me != null && Mathf.Abs ((me.transform.position - new Vector3 (dest.x, me.transform.position.y, dest.y)).magnitude) < 5f && !forRearch) {
			StartCoroutine (SelfResetDest ());
			forRearch=true;
		}*/
//		GetComponent<Rigidbody> ().AddForce (movement * speed * Time.deltaTime);
		if (me != null) {
		
			HashSet<GameObject> near = GetNearEnemy (me.transform.position, 10);
			if (near.Count != 0 && !forSearch && canReport) {
				if (isLeader) {
			//		toSubmission.SubmitReport (face, "敵を発見");
					canReport=false;
					StartCoroutine ("WaitReport");
				}

				forSearch = true;
			} else if (near.Count == 0) {
				forSearch = false;
			}

		}

	}

	public	void init (GameObject savaOb, int num, bool leader, int pmaxHp, GameObject mapicon, GameObject minimapicon, int sTime)
	{
		ob = savaOb;
		HP = maxHP = pmaxHp;
		//createMapIcon (mapicon, num);
		//createMinimapIcon (minimapicon);	
		isLeader = leader;
		myGroupNum = num;
		summonTime = sTime;
	}

	public void createMapIcon (GameObject map_icon, int num)
	{
		map_icon_ob = Instantiate (map_icon)as GameObject;
		map_icon_ob.transform.SetParent (GameObject.Find ("Organ/Map/Organ_sava").transform);
		map_icon_ob.GetComponent<mapicon> ().init (transform.FindChild ("Prepare").gameObject, num);
	}

	public void createMinimapIcon (GameObject minimap_icon)
	{
		minimap_icon_ob = Instantiate (minimap_icon) as GameObject;
		minimap_icon_ob.transform.SetParent (GameObject.Find ("Minimap/Field").transform);
		minimap_icon_ob.GetComponent<minimap_icon> ().init (transform.FindChild ("Prepare").gameObject);
	}

	public void create (Vector3 init_pos,bool lead)
	{
		me = Instantiate (ob, init_pos, Quaternion.identity )as GameObject;
		ob.GetComponent<Sava_controler> ().SetLeader (isLeader);
		me.transform.SetParent (transform);
		map_icon_ob.GetComponent<mapicon> ().init (me, myGroupNum);
		minimap_icon_ob.GetComponent<minimap_icon> ().init (me);

	}

	public void SetDestination (Vector2 destination)
	{
		dest = destination;
		forRearch = false;
	
	}
	
	public Vector2 GetDestination ()
	{
		return dest;	
	}

	private IEnumerator SelfResetDest ()
	{
		float startTime = Time.time;
		while (Time.time-startTime<5f) {
			yield return 0;        // 1フレーム後、再開
		}

		dest.x = me.transform.position.x;
		dest.y = me.transform.position.z;	
	}

	public void Damage (int a)
	{
		Vector3 b = Camera.main.WorldToScreenPoint (me.transform.position);
		b.y += 3f;
		GameObject bb = Instantiate(damageText,new Vector3(b.x,b.y,0f),Quaternion.identity) as GameObject;
		bb.GetComponent<Damage> ().DamageInt (a,minimap,me.transform.position);
		HP -= a;
	}

	public void imDead ()
	{

		EntrustLeader ();
		Destroy (map_icon_ob);
		Destroy (minimap_icon_ob);
		Destroy (headHP_ob);
		Destroy (sideHP_ob);
		Destroy (gameObject);
	}

	void EnterdEnemy ()
	{
	
	
	}

	public void setLeader (bool a)
	{
		isLeader = a;
	}

	HashSet<GameObject> GetNearMy (Vector3 pos, float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (pos, range);
		foreach (Collider b in a) {
			if (b.gameObject.tag == "My_sava" || b.gameObject.tag == "My_master")
				c.Add (b.gameObject);
		}
		return c;
	}

	HashSet<GameObject> GetNearEnemy (Vector3 pos, float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (pos, range);
		foreach (Collider b in a) {			
			if (b.gameObject.tag == "Enemy_sava" || b.gameObject.tag == "Enemy_master")
				c.Add (b.gameObject);
		}
		return c;
	}

	public void EntrustLeader ()
	{
		if (isLeader) {
			foreach (Transform child in mySava.transform) {
				if (child.GetComponent<mapicon> ().getGroupNum () == myGroupNum && !child.GetComponent<mapicon> ().isLeader ()) {
					child.GetComponent<mapicon> ().SetLeader (true);
					break;
				}
			}
		}
	}

	IEnumerable WaitReport ()
	{
		int a = 0;
		while (a<100) {
			a++;
			yield return 10;
		}
		canReport = true;
	}
	
}
