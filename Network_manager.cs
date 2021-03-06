﻿using UnityEngine;
using System.Collections;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Network_manager : Photon.PunBehaviour
{
	private PhotonView myPhotonView;
	
	// Use this for initialization
	public void Start ()
	{
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	
	public override void OnJoinedLobby ()
	{
		Debug.Log ("JoinRandom");
		PhotonNetwork.JoinRandomRoom ();
	}
	
	public override void OnConnectedToMaster ()
	{
		// when AutoJoinLobby is off, this method gets called when PUN finished the connection (instead of OnJoinedLobby())
		PhotonNetwork.JoinRandomRoom ();
	}
	
	public void OnPhotonRandomJoinFailed ()
	{
		RoomOptions roomOptions = new RoomOptions ();
		roomOptions.isVisible = true;
		roomOptions.isOpen = true;
		roomOptions.maxPlayers = 2;
		roomOptions.customRoomProperties = new Hashtable (){{"CustomProperties", "カスタムプロパティ"} };
		roomOptions.customRoomPropertiesForLobby = new string[] {"CustomProperties"};
		// ルームの作成
		PhotonNetwork.CreateRoom ("CustomPropertiesRoom", roomOptions, null);
		//PhotonNetwork.CreateRoom (null);
	}

	public override void OnJoinedRoom ()
	{
		PhotonNetwork.player.SetCustomProperties(new Hashtable() { { "ok", false } }); //playerのカスタムプロパティ
		GameObject selectChar = PhotonNetwork.Instantiate ("CharaSele", Vector3.zero, Quaternion.identity, 0);
		//DontDestroyOnLoad (PhotonNetwork.Instantiate ("Commons", Vector3.zero, Quaternion.identity, 0));
		//monster.GetComponent<myThirdPersonController>().isControllable = true;
		myPhotonView = selectChar.GetComponent<PhotonView> ();//monster.GetComponent<PhotonView>();
	}
	
	public void OnGUI ()
	{
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		
		if (PhotonNetwork.connectionStateDetailed == PeerState.Joined) {
			bool shoutMarco = GameLogic.playerWhoIsIt == PhotonNetwork.player.ID;
			
			if (shoutMarco && GUILayout.Button ("Marco!")) {
				myPhotonView.RPC ("Marco", PhotonTargets.All);
			}
			//if (!shoutMarco && GUILayout.Button ("Polo!")) {
			//	myPhotonView.RPC ("Polo", PhotonTargets.All);
			//}
		}
	}
}
