using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MG_func : MonoBehaviour
{
	int HP = 1000;
	int maxBarrier = 100;
	int barrier = 100;
	RectTransform HPBar;
	Text barrierOb;
	float HPUnit;
	bool canRecover;
	int recF = 0;
	int myNum;
	Color myColor;

	public struct order
	{
		public int summonTime;
		public Vector2 dest;
		public GameObject sava;
		public bool leader;
		public int gNum;
		public GameObject MMicon;
		public GameObject Micon;
		public GameObject sideHP;

		public order (int p1, GameObject p2, int gnum, bool p3, GameObject Mi, GameObject MMi, GameObject side, Vector2 de)
		{
			summonTime = p1;
			sava = p2;
			leader = p3;
			Micon = Mi;
			MMicon = MMi;
			dest = de;
			sideHP = side;
			gNum = gnum;
		}
	}
	
	GameObject minimapMG;
	GameObject mapMG;
	public GameObject headHP;
	public GameObject sideHP;
	Queue<order>[] sava_queue;
	private GameObject player;
	public GameObject prepareAction;
	Transform mySava;
	Transform savaSide;
	ParticleSystem[] prepare;
	order slip;
	bool creating;
	Vector3[] summonPos;
	int summonToken;
	int orderToken;
	Game_All_Init GameMaster;
	// Use this for initialization
	void Start ()
	{
		GameMaster = GameObject.Find ("GameMaster").GetComponent<Game_All_Init> ();
		sava_queue = new Queue<order>[6];

		mySava = GameObject.FindWithTag ("My_sava").transform;
		savaSide = GameObject.Find ("Player_info/Sava_side").transform;
		HPBar = GameObject.Find ("Player_info/Player_MG_HP/MG_HP_bar").GetComponent<RectTransform> ();
		HPUnit = HPBar.sizeDelta.x / HP;
		barrierOb = GameObject.Find ("Player_info/Player_MG_HP/Barrier").GetComponent<Text> ();
		creating = false;

		/*
		float x = (transform.position.x - 250f) / 5f;
		float y = (transform.position.z - 250f) / 5f;
		GameObject.Find ("MyMG").GetComponent<RectTransform> ().anchoredPosition = new Vector2 (x, y);
*/
		CreatePreparePos ();


		player.GetComponent<PlayerController> ().myNum = myNum;
		player.GetComponent<PlayerController> ().myColor = myColor;
	}

	public void init (GameObject p, string myTag, int num, Color co)
	{
		myNum = num;
		myColor = co;
		tag = myTag+"MG";
		player = p;
		player.GetComponent<PlayerController> ().myNum = myNum;
		player.GetComponent<PlayerController> ().myColor = myColor;
		player.GetComponent<PlayerController> ().init (myTag, myNum, myColor);

	}

	// Update is called once per frame
	void Update ()
	{
		if (sava_queue [summonToken].Count > 0 && !creating) {
			creating = true;
			slip = sava_queue [summonToken].Dequeue ();
			Invoke ("make", slip.summonTime);
			summonToken = summonToken > 4 ? 0 : summonToken + 1;
		}
		barrierOb.text = barrier.ToString ();
		HPBar.sizeDelta = new Vector2 (HP * HPUnit, HPBar.sizeDelta.y);

		if (canRecover && barrier < maxBarrier) {
			recF++;
			if (recF % 50 == 0) {
				recF = 0;
				barrier++;
			}
		} else if (barrier > maxBarrier) {
			recF++;
			if (recF % 20 == 0) {
				recF = 0;
				barrier--;
			}
		}

		//	GetComponent<UnityEngine.UI.Image>().enabled=true;			

	}

	void make ()
	{		
		int a = summonToken < 1 ? 5 : summonToken - 1;		
		//slip.sava.GetComponent<sava_base> ().create (summonPos [a], slip.leader);
		GameObject theSava = Instantiate (slip.sava, summonPos [a], Quaternion.identity) as GameObject;
		if (sava_queue [a].Count == 0)
			prepare [a].emissionRate = 0;

		slip.Micon.GetComponent<mapicon> ().init (theSava);
		slip.MMicon.GetComponent<minimap_icon> ().init (theSava);
		slip.sideHP.GetComponent<sava_side_info> ().SetOb (theSava);

		theSava.GetComponent<Sava_controler> ().init (slip.Micon, slip.MMicon, slip.sideHP, MakeHeadHP (theSava), slip.gNum, slip.leader, myNum);
		theSava.GetComponent<Sava_controler> ().SetDestination (slip.dest);
		theSava.transform.SetParent (mySava);
		creating = false;
	}

	public void Order (GameObject ob, int sTime, bool lea, GameObject Mi, GameObject MMi, int gNum, Vector2 dest)
	{
		prepare [orderToken].emissionRate = 100;
		
		sava_queue [orderToken].Enqueue (new order (sTime, ob, gNum, lea, createMapIcon (Mi, orderToken, gNum), createMinimapIcon (MMi, orderToken), createSideHP (ob, orderToken), dest));
		orderToken = orderToken > 4 ? 0 : orderToken + 1;
	}

	GameObject MakeHeadHP (GameObject me)
	{
		GameObject headHP_ob = Instantiate (headHP);
		headHP_ob.GetComponent<sava_head_HPLV> ().init (me);
		headHP_ob.transform.SetParent (GameObject.Find ("Sava_info").transform);
		return headHP_ob;
	}

	GameObject createSideHP (GameObject ob, int pre)
	{
		GameObject sideHP_ob = Instantiate (sideHP) as GameObject;		
		sideHP_ob.transform.SetParent (savaSide);
		sideHP_ob.GetComponent<sava_side_info> ().init (ob, prepare [pre].gameObject);
		return sideHP_ob;
	}

	public GameObject createMapIcon (GameObject map_icon, int targetPreNum, int gNum)
	{
		GameObject map_icon_ob = Instantiate (map_icon)as GameObject;
		map_icon_ob.transform.SetParent (GameObject.Find ("Organ/Map/Organ_sava").transform);
		map_icon_ob.GetComponent<mapicon> ().init (prepare [targetPreNum].gameObject, gNum);
		return map_icon_ob;
	}
	
	public GameObject createMinimapIcon (GameObject minimap_icon, int targetPreNum)
	{
		GameObject minimap_icon_ob = Instantiate (minimap_icon) as GameObject;
		minimap_icon_ob.transform.SetParent (GameObject.Find ("Minimap/Field").transform);
		minimap_icon_ob.GetComponent<minimap_icon> ().init (prepare [targetPreNum].gameObject);
		return minimap_icon_ob;
	}

	public void SetMyInfo (int myN, Color myC)
	{
		myNum = myN;
		myColor = myC;
	}

	public void Damage (int a)
	{
		canRecover = false;
		aa = 0;
		StartCoroutine ("Recovery");
		HP -= (int)(a * ((100 - barrier) / 100f));
		barrier -= 1;		
	}
	void CreatePreparePos(){
		summonPos = new Vector3[6];
		summonPos [0] = new Vector3 (35f, 2f, 50f);
		summonPos [1] = new Vector3 (40f, 2f, 50f);
		summonPos [2] = new Vector3 (45f, 2f, 50f);
		summonPos [3] = new Vector3 (50f, 2f, 45f);
		summonPos [4] = new Vector3 (50f, 2f, 40f);
		summonPos [5] = new Vector3 (50f, 2f, 35f);
		prepare = new ParticleSystem[6];
		for (int i =0; i<6; i++) {
			sava_queue [i] = new Queue<order> (){};
			GameObject a = PhotonNetwork.Instantiate ("Prepare", summonPos [i], Quaternion.identity,0) as GameObject;
			a.transform.SetParent (transform);
			prepare [i] = a.GetComponent<ParticleSystem> ();
			prepare [i].emissionRate = 0;
			
		}
	}

	int aa = 0;

	IEnumerator Recovery ()
	{
		while (aa<600) {
			aa++;
			yield return 0;
		}
		canRecover = true;
	}

}
