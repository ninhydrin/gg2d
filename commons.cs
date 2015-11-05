using UnityEngine;
using System.Collections;

public class commons :Photon.MonoBehaviour
{
	public bool[] ok;
	public bool startF;
	public bool myOk;
	// Use this for initialization
	void Start ()
	{
		ok = new bool[4];
		myOk = false;
		startF = false;
		DontDestroyOnLoad (gameObject);		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	void OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) {
			for (int i = 0; i<4; i++)
				stream.SendNext (ok [i]);
			stream.SendNext (startF);
			
		} else {
			for (int i = 0; i<4; i++)
				ok [i] = (bool)stream.ReceiveNext ();
			startF = (bool)stream.ReceiveNext ();
		}
	}
}
