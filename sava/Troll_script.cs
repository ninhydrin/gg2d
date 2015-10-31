using UnityEngine;
using System.Collections;

public class Troll_script : MonoBehaviour
{

	private Animator anim;							// キャラにアタッチされるアニメーターへの参照
	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照
	Sava_controler savaC;
	bool canDo = true;
	static int attack1 = Animator.StringToHash ("Base Layer.Attack_01");
	static int attack2 = Animator.StringToHash ("Base Layer.Attack_02");
	NavMeshAgent agent;
	
	// Use this for initialization
	void Start ()
	{
		savaC = GetComponent<Sava_controler> ();
		anim = GetComponent<Animator> ();
		agent = GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (savaC.HP <= 0)
			Destroy (this);
		else {
			currentBaseState = anim.GetCurrentAnimatorStateInfo (0);
			if (savaC.fighting && canDo) {
				if (agent.remainingDistance <= agent.stoppingDistance) {
					if (Random.value > 0.5) {
						anim.SetBool ("Attack1", true);
						transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (agent.destination - transform.position), 0.1f);
					} else {
						anim.SetBool ("Attack2", true);				
						transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (agent.destination - transform.position), 0.1f);						
					}		
					canDo = false;
					StartCoroutine (WaitDo ());
				}
			} else {
			
			}
			if (currentBaseState.nameHash == attack1) {
				anim.SetBool ("Attack1", false);		
			} else if (currentBaseState.nameHash == attack2) {		
				anim.SetBool ("Attack2", false);
			}
		}
	}

	IEnumerator WaitDo ()
	{
		int a = 0;
		while (a<100) {
			a++;
			yield return 0;
		}
		canDo = true;
	}
}
