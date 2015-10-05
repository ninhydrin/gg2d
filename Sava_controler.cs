using UnityEngine;
using System.Collections;

public class Sava_controler : MonoBehaviour
{

	private Animator anim;							// キャラにアタッチされるアニメーターへの参照
	private AnimatorStateInfo currentBaseState;			// base layerで使われる、アニメーターの現在の状態の参照
	Vector3 dest;
	float speed;
	bool reached;
	bool fighting;
	static int idleState = Animator.StringToHash ("Base Layer.Idle");
	static int walkState = Animator.StringToHash ("Base Layer.Idle");
	static int fightingState = Animator.StringToHash ("Base Layer.Idle");
	static int hitState = Animator.StringToHash ("Base Layer.Idle");
	static int downState = Animator.StringToHash ("Base Layer.Idle");
	static int dieState = Animator.StringToHash ("Base Layer.Idle");
	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator> ();
		dest = new Vector3 (50, transform.position.y, 50);
		speed = 0.07f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		currentBaseState = anim.GetCurrentAnimatorStateInfo (0);
		if (fighting) {

		} else {
			if ((dest - transform.position).magnitude < 3) {
				dest = transform.position;
				reached = true;	
			} else {
				reached = false;
			
			}

			if (reached) {
				anim.SetBool ("Walk", false);	
			} else {
				anim.SetBool ("Walk", true);
				Vector3 a = (dest - transform.position).normalized;
				transform.LookAt (dest);
				transform.localPosition += (new Vector3 (a.x, 0, a.z) * speed);
			}
		}

	}
}
