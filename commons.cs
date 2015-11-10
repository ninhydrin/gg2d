using UnityEngine;
using System.Collections;

public class commons :Photon.MonoBehaviour
{
	public bool ok;
	public bool startF;
	public bool myOk;
	GameObject parent;
	// Use this for initialization
	void Start ()
	{
		myOk = false;
		startF = false;
		DontDestroyOnLoad (gameObject);		
		parent = GameObject.Find ("commonsList");
		transform.SetParent (parent.transform);
		DontDestroyOnLoad (parent);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			stream.SendNext (ok);
			stream.SendNext (startF);
			
		} else {

			ok = (bool)stream.ReceiveNext ();
			startF = (bool)stream.ReceiveNext ();
		}
	}
}
