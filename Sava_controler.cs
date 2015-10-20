using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Sava_controler : MonoBehaviour
{

	private Animator anim;							// キャラにアタッチされるアニメーターへの参照
	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照

	public int maxHP;
	public string name;



	Vector3 dest;
	float speed;
	bool reached;
	bool fighting;
	NavMeshAgent agent;
	HashSet<GameObject> nearEnemy;
	GameObject myOb;



	static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int walkState = Animator.StringToHash ("Base Layer.Idle");
	static int fightingState = Animator.StringToHash ("Base Layer.Idle");
	static int damageState = Animator.StringToHash ("Base Layer.Damage");
	static int downState = Animator.StringToHash ("Base Layer.Idle");
	static int dieState = Animator.StringToHash ("Base Layer.Idle");
	// Use this for initialization
	void Start ()
	{
		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		dest = new Vector3 (0, transform.position.y, 0);
		speed = 0.07f;
		myOb = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentBaseState = anim.GetCurrentAnimatorStateInfo (0);

		Vector2 a = transform.parent.transform.GetComponent<sava_base> ().GetDestination ();
		
		dest = new Vector3 (a.x, transform.position.y, a.y);
		agent.SetDestination (dest);
		
		nearEnemy = GetNearAlly (5f);
		fighting = nearEnemy.Count != 0 ? true : false;
		if (fighting) {
			anim.SetBool ("Fighting", true);
		} else {
			anim.SetBool ("Fighting", false);			
			if (agent.remainingDistance <= agent.stoppingDistance) {
				reached = true;	
			} else {
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

	}

	HashSet<GameObject> GetNearAlly (float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (transform.position, range);
		foreach (Collider b in a) {
			if (b.gameObject.tag == "My_sava" || b.gameObject.tag == "Player" || b.gameObject.tag == "My_master")
				c.Add (b.gameObject);
		}
		return c;
	}
	
	HashSet<GameObject> GetNearEnemy (float range)
	{
		HashSet<GameObject> c = new HashSet<GameObject> ();
		Collider[] a = Physics.OverlapSphere (transform.position, range);
		foreach (Collider b in a) {			
			if (b.gameObject.tag == "Enemy_sava" || b.gameObject.tag == "Enemy_master")
				c.Add (b.gameObject);
		}
		return c;
	}

	public void Damaged (int num)
	{
		myOb.GetComponent<sava_base> ().Damage (num);
		anim.Play (damageState);		
		//animator.SetInteger ("Damage", num);
	}

}
