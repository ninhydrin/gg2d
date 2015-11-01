using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class organ_controller : Photon.MonoBehaviour
{
	PlayerController playerC;
	public bool ismenu;
	private int istype;
	public RectTransform rt;
	Transform[] menu_list;
	GameObject menu;
	KeyCode openMenu;
	KeyCode decision;
	Button buttons;
	cursor_controller cursorC;
	public bool itemSell;
	public bool summoning;
	public bool savaDictSetting;
	public mapicon target;
	public int targetGroup;
	Canvas organCanvas;
	int point_samon;
	GameObject mySava;
	bool canSell;
	bool canSell2;
	Stack<Vector2> destStack;
	Vector2  interchange;



	// Use this for initialization
	void Start ()
	{
		targetGroup = -10;
		summoning = false;
		savaDictSetting = false;
		itemSell = false;
		canSell = false;
		canSell2 = false;
		playerC = GetComponent<PlayerController> ();
		menu = GameObject.Find ("Menu");
		menu.GetComponent<UnityEngine.Canvas> ().enabled = false;
		organCanvas = GameObject.Find ("Organ").GetComponent<UnityEngine.Canvas> ();
		mySava = GameObject.Find ("Organ/Map/Organ_sava");
		ismenu = false;
		menu_list = new Transform[4];
		istype = 0;

		foreach (Transform child in menu.transform) {
			menu_list [istype] = child;
			istype++;			
		}
		istype = 0;
		buttons = GameObject.Find ("Player_info").GetComponent<Button> ();
		cursorC = GameObject.Find ("Organ_Cursor").GetComponent<cursor_controller> ();
		destStack = new Stack<Vector2> (){};
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (photonView.isMine) {
			if (playerC.organ) {		
				organCanvas.enabled = true;
				if (!canSell2) {
					StartCoroutine ("ItemSellCoolTime");
					canSell2 = true;
				}

		
				if (ismenu) {										//メニュー表示中
					menu.GetComponent<image_slide> ().SlideIn ();
					menu.GetComponent<UnityEngine.Canvas> ().enabled = true;			

					if (Input.GetKeyDown (buttons.LStick_Right)) {
						istype++;
						if (istype > 3)
							istype = 0;					
					}
					if (Input.GetKeyDown (buttons.LStick_Left)) {
						istype--;
						if (istype < 0)
							istype = 3;					
					}
					for (int i=0; i<4; i++) {
						if (i == istype) {
							menu_list [i].GetComponent<UnityEngine.Canvas> ().enabled = true;
						} else {
							menu_list [i].GetComponent<UnityEngine.Canvas> ().enabled = false;
						}
					}

					if (Input.GetKeyDown (buttons.LStick_Down)) {
						menu_list [istype].GetComponent<selector> ().Down ();
					}
					if (Input.GetKeyDown (buttons.LStick_Up)) {
						menu_list [istype].GetComponent<selector> ().Up ();			
					}
					if (Input.GetKeyDown (buttons.B_Button)) {
						MenuClose ();
					}
					if (istype == 0) {
						if (Input.GetKeyDown (buttons.A_Button)) {

							if (menu_list [istype].GetComponent<selector> ().isLocked ()) {
								if (menu_list [istype].GetComponent<selector> ().CanRelease ()) {

									menu_list [istype].GetComponent<selector> ().Release ();
								} else {

								}
							} else {
								if (menu_list [istype].GetComponent<selector> ().CanBuy ()) {
									point_samon = menu_list [istype].GetComponent<selector> ().point;
									summoning = true;
									ismenu = false;					
									MenuClose ();
									cursorC.SetMBdest (true);
									//	menu.GetComponent<UnityEngine.Canvas> ().enabled = false;
									AllListClose ();
									destStack.Clear ();
									interchange = Vector2.one * -10000f;
									destStack.Push (interchange);

								} else {


								}
							}
						}
						if (Input.GetKeyDown (buttons.Y_Button)) {
							if (menu_list [istype].GetComponent<selector> ().ReleasingNow ()) {
								menu_list [istype].GetComponent<selector> ().UnRelease ();
							}
						}
					} else if (istype == 1) {
						if (Input.GetKeyDown (buttons.A_Button)) {
							int k = playerC.IsEmp ();
							print (k);
							if (menu_list [istype].GetComponent<selector> ().CanBuy () && k != -1) {
								GameObject itembuy = menu_list [istype].GetComponent<selector> ().Buy ();
								playerC.AddItem (k, itembuy);
								playerC.SetItemSprite (k, itembuy.GetComponent<item> ().icon);
							}
						}

					} else if (istype == 2) {
						if (Input.GetKeyDown (buttons.A_Button)) {
							int k = playerC.IsEmp ();
							print (k);
							if (menu_list [istype].GetComponent<selector> ().CanBuy () && k != -1) {
								GameObject itembuy = menu_list [istype].GetComponent<selector> ().Buy ();
								playerC.AddItem (k, itembuy);
								playerC.SetItemSprite (k, itembuy.GetComponent<item> ().icon);
							}
						}
					} else if (istype == 3) {
						
					}


				} else if (summoning) {								//召喚準備中

					if (cursorC.isTarget () > 0) {           //グループ化
						targetGroup = cursorC.isTarget ();
						cursorC.SetMBdest (false);
						cursorC.SetMBisGroup (true);
						if (Input.GetKeyDown (buttons.A_Button)) {
							menu_list [istype].GetComponent<selector> ().summon (point_samon, GetDest (targetGroup), cursorC.isTarget ());
							summoning = false;
							cursorC.SetMBisGroup (false);
							cursorC.SetMBgroup (true);
						}
					} else {//目的地設定
						targetGroup = 0;
						cursorC.SetMBdest (true);
						cursorC.SetMBisGroup (false);
						if (Input.GetKeyDown (buttons.A_Button) && cursorC.isTarget () < 0) {
							cursorC.SetMBmove (false);
							cursorC.SetMBmarch (true);
							menu_list [istype].GetComponent<selector> ().summon (point_samon, cursorC.RealPos ());
							summoning = false;
						} 
					}

					if (Input.GetKeyDown (buttons.B_Button) || Input.GetKeyDown (buttons.Pad_Left)) {
						cursorC.SetMBmove (false);
						cursorC.SetMBcancel (true);
						targetGroup = 0;
						summoning = false;
					}




				} else if (savaDictSetting) {							//既存サーヴァントの目的地設定

				

					if (cursorC.isTarget () > 0 && targetGroup != cursorC.isTarget ()) { //グループ化
						cursorC.SetMBdest (false);					
						cursorC.SetMBisGroup (true);
						if (Input.GetKeyDown (buttons.A_Button)) {
							ConnectGroup (cursorC.isTarget ());
							savaDictSetting = false;	
							cursorC.SetMBisGroup (false);
							cursorC.SetMBgroup (true);
						}
					} else {    //目的地設定
						cursorC.SetMBdest (true);
						cursorC.SetMBisGroup (false);					
						if (Input.GetKeyDown (buttons.A_Button) && cursorC.isTarget () < 0) {

							DictSet (cursorC.RealPos ());
							savaDictSetting = false;
							cursorC.SetMBmove (true);
						}
					}


					if (Input.GetKeyDown (buttons.B_Button) || Input.GetKeyDown (buttons.Pad_Left)) {
						cursorC.SetMBdest (false);
						cursorC.SetMBisGroup (false);
						cursorC.SetMBcancel (true);
						savaDictSetting = false;
					}




				} else if (itemSell) {										//アイテム売却画面
					if (Input.GetKeyDown (buttons.B_Button)) {
						cursorC.SetMBdest (true);
						itemSell = false;
					}
			


				} else {													//何もしていない状態
					if (Input.GetKey (buttons.RB)) {
						if (cursorC.isTarget () > 0) {
							cursorC.SetMBdest (true);
							if (Input.GetKeyDown (buttons.A_Button)) {
								Discharge ();
							}
						} else {
							cursorC.SetMBdest (false);
						}
					} else {
						cursorC.SetMBdest (false);
						if (Input.GetKeyDown (buttons.A_Button)) { 
							if (cursorC.isTarget () > 0) {
								cursorC.SetMBdest (true);
								targetGroup = cursorC.isTarget ();
								savaDictSetting = true;
								SelectTarget ();
								interchange = Vector2.one * -10000f;
					
							}
				
						} else if (Input.GetKeyDown (buttons.Y_Button)) {
							itemSell = true;
						}
				
						MenuClose ();
						if (Input.GetKeyDown (buttons.X_Button)) {
							if (!summoning) {
								ismenu = true;
								istype = 0;
							}
						}
						if (Input.GetKeyDown (buttons.B_Button)) {
							playerC.organ = false;
						}

						if (Input.GetKeyDown (buttons.LT)) {

						}
					}
				}
						

			} else {										//オルガンを閉じている状態
				MenuClose ();
				organCanvas.enabled = false;
				GameObject.Find ("Menu").GetComponent<UnityEngine.Canvas> ().enabled = false;
				AllListClose ();
				summoning = false;
				canSell = false;
				canSell2 = false;
			}
		}
	}
		
	void MenuClose ()
	{
		//	ismenu = false;
		menu.GetComponent<image_slide> ().SlideOut ();
		ismenu = false;
	}

	void AllListClose ()
	{
		for (int i=0; i<4; i++) {					
			menu_list [i].GetComponent<UnityEngine.Canvas> ().enabled = false;					
		}
	}

	public void SelectTarget ()
	{
		foreach (Transform child in mySava.transform) {
			if (child.GetComponent<mapicon> ().getGroupNum () == targetGroup)
				
				StartCoroutine (child.GetComponent<mapicon> ().ImSelected ());
		}
		
	}

	public void DictSet (Vector2 a)
	{
		foreach (Transform child in mySava.transform) {
			if (child.GetComponent<mapicon> ().getGroupNum () == targetGroup)
				child.GetComponent<mapicon> ().SetDest (a);
		}
	}

	Vector2 DictGet ()
	{
		foreach (Transform child in mySava.transform) {
			if (child.GetComponent<mapicon> ().getGroupNum () == targetGroup) {
				return child.GetComponent<mapicon> ().GetDest ();
			}
		}
		return Vector2.zero;
	}

	public void ConnectGroup (int a)
	{
		foreach (Transform child in mySava.transform) {
			Vector2 b = GetDest (a);
			if (child.GetComponent<mapicon> ().getGroupNum () == targetGroup) {
				child.GetComponent<mapicon> ().SetGroupNum (a);
				child.GetComponent<mapicon> ().SetLeader (false);
				child.GetComponent<mapicon> ().SetDest (b);
			}
		}
	}

	public Vector2 GetDest (int a)
	{
		Vector2 b = Vector2.zero;
		foreach (Transform child in mySava.transform) {
			if (child.GetComponent<mapicon> ().getGroupNum () == a) {
				b = child.GetComponent<mapicon> ().GetDest ();
				break;
			}
		}
		return b;
	}

	void Discharge ()
	{

		int k = menu_list [0].GetComponent<selector> ().NewGroupNum ();
		GameObject a = cursorC.targetOb;
		a.GetComponent<mapicon> ().SetGroupNum (k);
		a.GetComponent<mapicon> ().SetLeader (true);
		a.GetComponent<mapicon> ().EntrustLeader ();

	}

	IEnumerator ItemSellCoolTime ()
	{
		int coolTime = 0;
		while (coolTime<90) {
			coolTime++;
			if (!playerC.organ)
				yield break;
			yield return 0;
		}
		canSell = true;
	}

}
