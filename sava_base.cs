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
	// Use this for initialization
	void Start ()
	{
		mySava = GameObject.Find ("Organ/Map/Organ_sava");
		face = Resources.Load<Sprite> ("query-chan");
		toSubmission = GameObject.Find ("Player_info/Sava_report").GetComponent<sava_report> ();
		me = null;
		canReport = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (me != null && Mathf.Abs (me.transform.position.magnitude - dest.magnitude) < 5f)
			StartCoroutine (SelfResetDest ());
//		GetComponent<Rigidbody> ().AddForce (movement * speed * Time.deltaTime);
		if (HP <= 0)
			imDead ();
		if (me != null) {
		
			HashSet<GameObject> near = GetNearEnemy (me.transform.position, 10);
			if (near.Count != 0 && !forSearch && canReport) {
				if (isLeader) {
					toSubmission.SubmitReport (face, "敵を発見");
					canReport=false;
					StartCoroutine ("WaitReport");
				}
				forSearch = true;
			} else if (near.Count == 0) {
				forSearch = false;
			}

		}

	}

	public	void init (int num, bool leader, int pmaxHp, GameObject mapicon, GameObject minimapicon, int sTime)
	{
		HP = maxHP = pmaxHp;
		createMapIcon (mapicon, num);
		createMinimapIcon (minimapicon);	

		sideHP_ob = Instantiate (sideHP) as GameObject;		
		sideHP_ob.transform.SetParent (GameObject.Find ("Player_info/Sava_side").transform);
		sideHP_ob.GetComponent<sava_side_info> ().init (this.gameObject);

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

	public void create ()
	{
		Destroy (transform.FindChild ("Prepare").transform.gameObject);
		Vector3 init_pos = new Vector3 (50f, 01f, 50f);

		me = Instantiate (ob, init_pos, Quaternion.identity )as GameObject;
		me.transform.SetParent (transform);
		map_icon_ob.GetComponent<mapicon> ().init (me, myGroupNum);
		headHP_ob = Instantiate (headHP);
		headHP_ob.GetComponent<sava_head_HPLV> ().init (me);
		headHP_ob.transform.SetParent (GameObject.Find ("Sava_info").transform);
		minimap_icon_ob.GetComponent<minimap_icon> ().init (me);

	}

	public void SetDestination (Vector2 destination)
	{
		dest = destination;
	
	}

	public void ResetDestination ()
	{
		//dest = null;
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
		HP -= a;
	}

	public void Repair (int a)
	{	
		if (HP + a > maxHP) {
			HP = maxHP;
		} else {
			HP += a;
		}
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
