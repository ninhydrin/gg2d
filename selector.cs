using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class selector : MonoBehaviour
{

	RectTransform rt;
	Image myselector;
	Vector2 init_pos;
	float updown ;
	public int point;
	Transform[] entry_list;
	Transform entrys;
	int entry_num;
	MG_func MG;
	organ_controller organ;
	int savaGroupNum;
	bool coflag;
	Mana_manager Mana;

	public GameObject savant;
	public bool cantReleasing;
	public string myTag;
	// Use this for initialization


	void Start ()
	{
		entrys = transform.FindChild ("Entry_list");
		entry_num = entrys.childCount;
		rt = transform.FindChild ("Menu_selector").GetComponent<RectTransform> ();
		myselector = transform.FindChild ("Menu_selector").GetComponent<Image> ();
		Mana = GameObject.Find ("Mana").GetComponent<Mana_manager> ();
		init_pos = new Vector2 (0f, -20f);
		rt.anchoredPosition = init_pos;

		updown = 21f;
		point = 0;
		MG = GameObject.FindGameObjectWithTag ("PlayerMG").GetComponent<MG_func> ();

		entry_list = new Transform [entry_num];
		int count = 0;
		foreach (Transform child in entrys) {
			child.GetComponent<RectTransform> ().anchoredPosition = init_pos;
			init_pos.y -= 21f;
			entry_list [count] = child;
			count++;
		
		}
		organ = GameObject.FindWithTag ("Player").GetComponent<organ_controller> ();
		init_pos = new Vector2 (0f, -20f);
		rt.anchoredPosition = init_pos;
		savaGroupNum = 1;
		coflag = false;
		cantReleasing = false;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (!organ.ismenu) {
			rt.anchoredPosition = init_pos;
			point = 0;
		}
		if (GetComponent<Canvas> ().enabled && !coflag) {
			StartCoroutine (ImSelected ());
			coflag = true;
		}
		if (!GetComponent<Canvas> ().enabled) {
			coflag = false;
		}
	}

	public void Down ()
	{
		if (point < entry_num - 1) {
			rt.anchoredPosition = new Vector2 (0f, rt.anchoredPosition.y - updown);
			point++;
		}
	}

	public void Up ()
	{
		if (point > 0) {
			rt.anchoredPosition = new Vector2 (0f, rt.anchoredPosition.y + updown);
			point--;
		}
	}

	public void Reset ()
	{
		point = 0;
		rt.anchoredPosition = init_pos;
	}

	public void summon (int point_sava,Vector2 dest,int teamnum = -100)
	{
		organ_menu_entry theEntry = entry_list [point_sava].GetComponent<organ_menu_entry> ();
		Mana.RemoveMana (theEntry.cost);

		Vector3 init_pos = new Vector3 (50f, 0f, 50f);
		bool leader = true;
		int gnum;
		if (teamnum == -100) {
			gnum = savaGroupNum;
			savaGroupNum++;		
		} else {
			gnum = teamnum;
			leader=false;
		}
		for (int i =0; i<theEntry.HowMany; i++) {
			MG.Order (theEntry.sava,theEntry.sumonTime,leader,theEntry.map_icon,theEntry.minimap_icon,gnum,dest);
			leader = false;			
		}
	}

	public GameObject Buy ()
	{
		organ_menu_entry theEntry = entry_list [point].GetComponent<organ_menu_entry> ();
		Mana.RemoveMana (theEntry.cost);
		return Instantiate (theEntry.sava) as GameObject;
	}

	public bool CanRelease ()
	{
		if (Mana.GetMana () < entry_list [point].GetComponent<organ_menu_entry> ().releaseCost || entry_list [point].GetComponent<organ_menu_entry> ().releasing) {
			return false;
		} else {
			return true;
		}	
	}

	public bool isLocked ()
	{	
		return 	entry_list [point].GetComponent<organ_menu_entry> ().sava_lock;

	}

	public void Release ()
	{
		Mana.RemoveMana (entry_list [point].GetComponent<organ_menu_entry> ().releaseCost);
		entry_list [point].GetComponent<organ_menu_entry> ().iRelease ();
	}

	public void UnRelease ()
	{
		Mana.AddMana (entry_list [point].GetComponent<organ_menu_entry> ().releaseCost);
		entry_list [point].GetComponent<organ_menu_entry> ().StopRelease ();
	}

	public bool CanBuy ()
	{
		return (Mana.GetMana () < entry_list [point].GetComponent<organ_menu_entry> ().cost) ? false : true;
	}

	public void show ()
	{
		int count = 0;
		foreach (Transform child in transform) {
			//child is your child transform
			
			print ("Child[" + count + "]:" + child.name);
			count++;
		}
	}

	public bool ReleasingNow ()
	{
		return entry_list [point].GetComponent<organ_menu_entry> ().releasing;
	}

	public IEnumerator ImSelected ()
	{
		Color or = myselector.color;

		float maxcolor = 0.7f;
		float num = 0.05f;
		while (transform.GetComponent<Canvas>().enabled) {
			if (maxcolor < myselector.color.b)
				num = -0.05f;
			else if (myselector.color.b <= 0.3)
				num = 0.05f;
			myselector.color = new Color (myselector.color.r, myselector.color.g, myselector.color.b + num, 0.5f);
			yield return 10;        // 1フレーム後、再開
		}
		myselector.color = or;
	}

	public int NewGroupNum ()
	{
		int a = savaGroupNum++;
		return a;
	}

	public void BuyItem ()
	{
		if (entry_list [point] != null) {
			Destroy (entry_list [point].gameObject);
			AdjustList ();
		}
	}

	void AdjustList ()
	{
		int a = point;
		entry_num--;
		int count = 0;
		for (int i = a; i<entry_num; i++) {
			entry_list [i] = entry_list [i + 1];
			entry_list [i].GetComponent<RectTransform> ().anchoredPosition -= new Vector2 (0, -21f);								
		}
		
		if (a == entry_num) {
			Up ();			
		}
	}

}
