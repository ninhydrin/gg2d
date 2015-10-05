using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class sava_report : MonoBehaviour
{

	struct Report
	{
		public Sprite face;
		public string contents;
		
		public Report (Sprite a, string b)
		{
			face = a;
			contents = b;
		}
	}
	GameObject report_face;
	GameObject report_text;
	Queue<Report> report_queue;
	Canvas me;
	bool showing;
	// Use this for initialization
	void Start ()
	{
		report_face = transform.FindChild ("Report_face").gameObject;
		report_text = transform.FindChild ("Report_text").gameObject;
		report_queue = new Queue<Report> ();
		me = gameObject.GetComponent<Canvas> ();
		me.enabled = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (report_queue.Count > 0 && !showing) {
			showing = true;
			Report nowAnalysis = report_queue.Dequeue ();
			PrintReport (nowAnalysis.face, nowAnalysis.contents);
			StartCoroutine ("Reported");
		}

	}

	public IEnumerator Reported ()
	{
		int a = 0;
		me.enabled = true;
		while (a<100) {
			a++;
			yield return 10;
		}
		me.enabled = false;
		showing = false;	
	}

	void PrintReport (Sprite a, string b)
	{
		report_face.GetComponent<Image> ().sprite = a;
		report_text.GetComponent<Text> ().text = b;
	}

	public void SubmitReport (Sprite a, string b)
	{
		report_queue.Enqueue (new Report (a, b));
	}
}
