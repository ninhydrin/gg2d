using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class organ_menu_entry : MonoBehaviour
{

	public GameObject sava;
	public int cost;
	public Sprite face;
	public int sumonTime;
	public bool sava_lock;
	public int HowMany;
	public int releaseCost;
	public float releaseTime;
	public int maxHp;
	public GameObject minimap_icon;
	public GameObject map_icon;


	Color imLocking;
	Color imLockingCant;
	Color imFree;
	Color imFreeCant;
	Color ReleacingNow;
	bool Cando;
	Text CostText;
	Text SnameText;
	Image Myface;
	Mana_manager myMana;
	GameObject SavaInfo;
	public bool releasing;

	// Use this for initialization
	void Start ()
	{
		imLocking = Color.red;
		imLockingCant = new Color (0.7f, 0.5f, 0.5f, 1f);
		imFree = Color.white;
		imFreeCant = Color.gray;
		ReleacingNow = Color.yellow;
		CostText = transform.FindChild ("Cost").GetComponent<Text> ();
		if (releaseCost > 0) {
			CostText.text = releaseCost.ToString ();
		} else {
			CostText.text = cost.ToString ();
		}
		SnameText = transform.FindChild ("Sname").GetComponent<Text> ();
		Myface = transform.FindChild ("Face_image").GetComponent<Image> ();
		Myface.sprite = face;
		Cando = true;
		releasing = false;
		myMana = GameObject.Find ("Mana").GetComponent<Mana_manager> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (sava_lock && myMana.GetMana () < releaseCost && !releasing) {
			SetTextColor (imLockingCant);
		} else if (sava_lock && myMana.GetMana () >= releaseCost && !releasing) {
			SetTextColor (imLocking);
		} else if (!sava_lock && myMana.GetMana () >= cost && !releasing) {
			SetTextColor (imFree);
		} else if (releasing) {
			SetTextColor (ReleacingNow);
		} else {
			SetTextColor (imFreeCant);
		}
		if (!sava_lock)
			releasing = false;
	}

	void SetTextColor (Color aaa)
	{
		CostText.color = aaa;
		SnameText.color = aaa;
	}

	public void iRelease ()
	{
		releasing = true;
		StartCoroutine ("ImReleasing");
	}

	public void StopRelease ()
	{
		StopCoroutine ("ImReleasing");
		releasing = false;
		CostText.text = releaseCost.ToString ();
	}

	private IEnumerator ImReleasing ()
	{
		float startTime = Time.time;
		while ((Time.time - startTime) < releaseTime) {
			CostText.text = ((int)(releaseTime - (Time.time - startTime)) + 1).ToString ();
			yield return 0;  
		}
		sava_lock = false;
		releasing = false;
		CostText.text = cost.ToString ();
	}
}
