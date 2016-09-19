﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

	List<GameObject> roomList = new List<GameObject>();

	[SerializeField]
	private Text status;
	[SerializeField]
	private GameObject roomListItemPrefab;
	[SerializeField]
	private Transform roomListParent;


	private NetworkManager networkManager;



	void Start () {
		networkManager = NetworkManager.singleton;
		if (networkManager.matchMaker == null) {
			networkManager.StartMatchMaker();
		}

		RefreshRoomList();
	}

	public void RefreshRoomList () {
		ClearRoomList();
		networkManager.matchMaker.ListMatches(0, 20, "", true, 0,0, OnMatchList);
		status.text = "Loading...";
	}

	public void OnMatchList (bool succes, string extendedInfo, List<MatchInfoSnapshot> matchList) {
		status.text = "";

		if (succes == false || matchList == null) {
			status.text = "Couldn't find any rooms: " + extendedInfo + ".";
			return;
		}

		foreach (MatchInfoSnapshot match in matchList) {
			GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
			_roomListItemGO.transform.SetParent(roomListParent);

			RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
			if (_roomListItem != null){
				_roomListItem.Setup(match, JoinRoom);
			}



			// have a component sit on the GO thatwill take care of setting up name/ amount of users & setting up callback that will join the game
			roomList.Add(_roomListItemGO);
		}
		if (roomList.Count == 0) {
			status.text = "No rooms at the moment.";
		}
	}

	void ClearRoomList() {
		for (int i = 0; i < roomList.Count; i++) {
			Destroy (roomList[i]);
		}

		roomList.Clear();
	}

	public void JoinRoom (MatchInfoSnapshot _match) {
		Debug.Log ("joining" + _match.name);
		networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
		ClearRoomList();
		status.text = "Joining...";
	}
}
