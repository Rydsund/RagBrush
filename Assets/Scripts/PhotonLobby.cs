using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class PhotonLobby : MonoBehaviourPunCallbacks, ILobbyCallbacks
{
	public static PhotonLobby lobby;

	public string roomName;
	public int roomSize;
	public GameObject roomListingPrefab;
	public Transform roomsPanel;


	private void Awake()
	{
		lobby = this;//Creates a singleton, lives within the main menue scene.
	}


	// Start is called before the first frame update
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings(); //Connect to master photon server.
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Player has connected to Photon master server");
		PhotonNetwork.AutomaticallySyncScene = true;
		PhotonNetwork.NickName = "Player" + Random.Range(0, 1000);
	}

	public override void OnRoomListUpdate(List<RoomInfo> roomList)
	{
		base.OnRoomListUpdate(roomList);
		RemoveRoomListings();
		foreach (RoomInfo room in roomList)
		{
			ListRoom(room);
		}
	}

	void RemoveRoomListings()
	{
		while (roomsPanel.childCount != 0)
		{
			Destroy(roomsPanel.GetChild(0).gameObject);
		}
	}

	void ListRoom(RoomInfo room)
	{
		if (room.IsOpen && room.IsVisible)
		{
			GameObject tempListing = Instantiate(roomListingPrefab, roomsPanel);
			RoomButton tempButton = tempListing.GetComponent<RoomButton>();
			tempButton.roomName = room.Name;
			tempButton.roomSize = room.MaxPlayers;
			tempButton.SetRoom();
		}
	}

	public void CreateRoom()
	{
		RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)roomSize };
		PhotonNetwork.CreateRoom(roomName, roomOps);
		Debug.Log("Room Created");
	}

	public override void OnCreateRoomFailed(short returnCode, string message)
	{
		Debug.Log("Faild to Create Room, no open game available");
		//CreateRoom();
		base.OnCreateRoomFailed(returnCode, message);
	}

	public void OnRoomNameChanged(string nameIn)
	{
		roomName = nameIn;
	}

	public void OnRoomSizeChanged(string sizeIn)
	{
		roomSize = int.Parse(sizeIn);
	}
	public void JoinLobbyOnClick()
	{
		if (!PhotonNetwork.InLobby)
		{
			PhotonNetwork.JoinLobby();
		}
	}
}
