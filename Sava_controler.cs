using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sava_controler : MonoBehaviour
{

	private Animator anim;							// キャラにアタッチされるアニメーターへの参照
	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

	public GameObject damageText;
	public int maxHP;
	public Sprite myFace;
	public string myName;
	
	public int HP {
		get;
		private set;
	}

	public int LV {
		get;
		private set;
	}

	int myTeamNum;
	int myGroupNum;
	GameObject mapIcon;
	GameObject mMapIcon;
	GameObject headHP;
	GameObject sideHP;

	bool leader;
	int myCost;
	public float searchDest;
	public float stopDest;
	public float attackDest;
	public float standDest;
	Vector3 dest;
	Vector3 tempDest;
	float speed;
	bool reached;
	public bool fighting;
	NavMeshAgent agent;
	HashSet<GameObject> nearEnemy;
	GameObject Target;
	GameObject minimap;
	sava_report toSubmission;
	bool canReport;
	static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int walkState = Animator.StringToHash ("Base Layer.Idle");
	static int fightingState = Animator.StringToHash ("Base Layer.Fighting");
	static int damageState = Animator.StringToHash ("Base Layer.Damage");
	static int downState = Animator.StringToHash ("Base Layer.Idle");
	static int dieState = Animator.StringToHash ("Base Layer.Die");
	// Use this for initialization

	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		minimap = GameObject.Find ("Minimap");
		toSubmission = GameObject.Find ("Player_info/Sava_report").GetComponent<sava_report> ();
		canReport = true;		
		HP = maxHP;
		MakeHeadHP ();
	}

	void init ()
	{



	}

	// Update is called once per frame
	void Update ()
	{
		if (HP > 0) {
			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);
			
			//nearEnemy = GetNearAlly (8f);
			nearEnemy = GetNearEnemy (8f);
			fighting = nearEnemy.Count != 0 ? true : false;

			if (nearEnemy.Count != 0 && !fighting && canReport) {
				if (leader) {
					toSubmission.SubmitReport (myFace, "敵を発見");
					canReport = false;
					StartCoroutine ("WaitReport");
				}
			}


			if (fighting) {
				agent.stoppingDistance = attackDest;
				if (!Target) {
					foreach (GameObject setIn in nearEnemy) {
						Target = setIn;
						tempDest = setIn.transform.position;
						break;
					}
				} else {
					tempDest = Target.transform.position;
				}

				if (agent.remainingDistance <= agent.stoppingDistance) {
					anim.SetBool ("Walk", false);				
					anim.SetBool ("Fighting", true);
					agent.Stop ();
					//transform.LookAt(Target.transform.position);
					transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (Target.transform.position - transform.position), 0.1f);
				} else if (currentBaseState.nameHash == walkState || currentBaseState.nameHash == fightingState) {
					anim.SetBool ("Fighting", false);					
					anim.SetBool ("Walk", true);
					agent.Resume ();
				}

			} else {
				agent.Resume ();
				anim.SetBool ("Fighting", false);	
				Target = null;
				agent.stoppingDistance = stopDest;
				tempDest = new Vector3 (dest.x, transform.position.y, dest.y);
			
				if (agent.remainingDistance <= agent.stoppingDistance) {
					reached = true;	
					agent.stoppingDistance = standDest;				
				} else {
					agent.stoppingDistance = stopDest;
					reached = false;
				}

				if (reached) {
					anim.SetBool ("Walk", false);	
				} else {
					anim.SetBool ("Walk", true);
					//Vector3 a = (dest - transform.position).normalized;
					//transform.LookAt (dest);
					//transform.localPosition += (new Vector3 (a.x, 0, a.z) * speed);
				}
			}
			if(currentBaseState.nameHash == damageState){
				agent.Stop();
			}else{
				agent.Resume();
			}

			if (dest != tempDest)
				agent.SetDestination (tempDest);
		} else {
			Destroy (agent);
			Destroy (gameObject.GetComponent<Rigidbody> ());
			imDead ();
			anim.SetBool ("Die", true);
			anim.Play (dieState);
		
		}
	}

	public void init (GameObject Mi, GameObject MMi, GameObject SS,int gnum, bool le, int myNum)
	{
		mapIcon = Mi;
		mMapIcon = MMi;
		sideHP = SS;
		myGroupNum = gnum;
		leader = le;
		myTeamNum = myNum;
		gameObject.tag = (myNum).ToString()+"P_Sava";
	}
	void MakeHeadHP ()
	{
		headHP = PhotonNetwork.Instantiate ("Sava_HP_LV", Vector3.zero, Quaternion.identity, 0) as GameObject;		
		headHP.GetComponent<sava_head_HPLV> ().init (gameObject);
		headHP.transform.SetParent (GameObject.Find ("Sava_info").transform);
	}

	HashSet<GameObject> GetNearAlly (float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (transform.position, range);
		foreach (Collider b in a) {
			if (b.gameObject.tag ==  myTeamNum.ToString()+"P_Sava" || b.gameObject.tag == myTeamNum.ToString()+ "P_Master" ||b.gameObject.tag == myTeamNum.ToString()+"P_MG")
				c.Add (b.gameObject);
		}
		return c;
	}
	
	HashSet<GameObject> GetNearEnemy (float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (transform.position, range);
		foreach (Collider b in a) {
			if (b.gameObject.tag == "Ghost" && b.gameObject.GetComponent<ghost_base> ().dominator != myTeamNum)
				c.Add(b.gameObject);
			if (b.gameObject.tag == "Enemy_sava" || b.gameObject.tag == "Enemy_master")
				c.Add (b.gameObject);
		}
		return c;
	}

	public void SetLeader (bool a = false)
	{
		leader = a;
	}

	public void SetGnum (int num)
	{
		myGroupNum = num;
	}

	public void Damaged (int num)
	{
		Vector3 b = Camera.main.WorldToScreenPoint (transform.position);
		b.y += 3f;
		GameObject bb = Instantiate (damageText, new Vector3 (b.x, b.y, 0f), Quaternion.identity) as GameObject;
		bb.GetComponent<Damage> ().DamageInt (num, minimap, transform.position);
		HP -= num;
		anim.Play (damageState);		
		//animator.SetInteger ("Damage", num);
	}


	public void Repair (int a)
	{	
		if (HP + a > maxHP) {
			HP = maxHP;
		} else {
			HP += a;
		}
	}

	public void SetDestination (Vector2 destination)
	{
		dest = destination;
	}

	public bool isLeader ()
	{
		return leader;
	}

	public int GetGroupNum ()
	{
		return myGroupNum;
	}

	public Vector2 GetDestination ()
	{
		return dest;	
	}



	public void imDead ()
	{		
		EntrustLeader ();
		Destroy (mapIcon);
		Destroy (mMapIcon);
		Destroy (headHP);
		Destroy (sideHP);
		//Destroy (this);	
		//Destroy (gameObject);
		gameObject.tag = "Die";
	}

	public void EntrustLeader ()
	{
		if (leader) {
			foreach (Transform child in GameObject.Find("My_sava").transform) {
				if (child.GetComponent<Sava_controler> ().GetGroupNum () == myGroupNum && !child.GetComponent<Sava_controler> ().isLeader () && child.gameObject.tag != "Die") {
					child.GetComponent<Sava_controler> ().SetLeader (true);
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
