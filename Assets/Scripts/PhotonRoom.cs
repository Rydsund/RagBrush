using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using Photon.Pun;

public class PhotonRoom : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
	//Room info
	public static PhotonRoom room;
	private PhotonView PV;

	public bool isGameLoaded;
	public int currentScene;
	public int multiplayerScene;

	private Player[] photonPlayers;
	public int playersInRoom;
	public int myNumberInRoom;

	public int playerInGame;


	// Delay start
	private bool readyToCount;
	private bool readyToStart;
	public float startingTime;
	private float lessThanMaxPlayers;
	private float atMaxPlayers;
	private float timeToStart;


	private void Awake()
	{
		//Set up singleton
		if (PhotonRoom.room == null)
		{
			PhotonRoom.room = this;
		}
		else
		{
			if (PhotonRoom.room != this)
			{
				Destroy(PhotonRoom.room.gameObject);
				PhotonRoom.room = this;
			}
		}
		DontDestroyOnLoad(this.gameObject);
		PV = GetComponent<PhotonView>();
	}

	public override void OnEnable()
	{
		base.OnEnable();
		PhotonNetwork.AddCallbackTarget(this);
		SceneManager.sceneLoaded += OnsceneFinishedLoading;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		PhotonNetwork.RemoveCallbackTarget(this);
		SceneManager.sceneLoaded -= OnsceneFinishedLoading;
	}

	void Start()
	{
		PV = GetComponent<PhotonView>();
		readyToCount = false;
		readyToStart = false;
		lessThanMaxPlayers = startingTime;
		atMaxPlayers = 6;
		timeToStart = startingTime;
	}

	void Update()
	{
		if (MultiplayerSetting.multiplayerSetting.delayStart)
		{
			if (playersInRoom == 1)
			{
				RestartTimer();
			}
			if (!isGameLoaded)
			{
				if (readyToStart)
				{
					atMaxPlayers -= Time.deltaTime;
					lessThanMaxPlayers = atMaxPlayers;
					timeToStart = atMaxPlayers;
				}
				else if (readyToCount)
				{
					lessThanMaxPlayers -= Time.deltaTime;
					timeToStart = lessThanMaxPlayers;
				}
				Debug.Log("Dislplay time to start to the players" + timeToStart);
				if (timeToStart <= 0)
				{
					StartGame();
				}
			}
		}
	}

	public override void OnJoinedRoom()
	{
		//Sets player data when we join the room
		base.OnJoinedRoom();
		Debug.Log("You are in a room");
		photonPlayers = PhotonNetwork.PlayerList;
		playersInRoom = photonPlayers.Length;
		myNumberInRoom = playersInRoom;
		PhotonNetwork.NickName = myNumberInRoom.ToString();
		if (MultiplayerSetting.multiplayerSetting.delayStart)
		{
			Debug.Log("Displayer players in room out of max players possible(" + playersInRoom + ":" + MultiplayerSetting.multiplayerSetting.maxPlayer + ")");
			if (playersInRoom > 1)
			{
				readyToCount = true;
			}
			if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayer)
			{
				readyToStart = true;
				if (!PhotonNetwork.IsMasterClient)
					return;
				PhotonNetwork.CurrentRoom.IsOpen = false;
			}
		}
		else
		{
			StartGame();
		}
	}

	public override void OnPlayerEnteredRoom(Player newPlayer)
	{
		base.OnPlayerEnteredRoom(newPlayer);
		Debug.Log("A new player has joined the room");
		photonPlayers = PhotonNetwork.PlayerList;
		playersInRoom++;
		if (MultiplayerSetting.multiplayerSetting.delayStart)
		{
			Debug.Log("Displayer players in room out of max players possible(" + playersInRoom + ":" + MultiplayerSetting.multiplayerSetting.maxPlayer + ")");
			if (playersInRoom > 1)
			{
				readyToCount = true;
			}
			if (playersInRoom == MultiplayerSetting.multiplayerSetting.maxPlayer)
			{
				readyToStart = true;
				if (!PhotonNetwork.IsMasterClient)
					return;
				PhotonNetwork.CurrentRoom.IsOpen = false;
			}
		}
	}

	void StartGame()
	{
		isGameLoaded = true;
		if (!PhotonNetwork.IsMasterClient)
			return;
		if (MultiplayerSetting.multiplayerSetting.delayStart)
		{
			PhotonNetwork.CurrentRoom.IsOpen = false;
		}
		PhotonNetwork.LoadLevel(MultiplayerSetting.multiplayerSetting.multiplayerScene);


		//PhotonNetwork.LoadLevel(multiplayerScene);
	}

	void RestartTimer()
	{
		lessThanMaxPlayers = startingTime;
		timeToStart = startingTime;
		atMaxPlayers = 6;
		readyToCount = false;
		readyToStart = false;
	}

	void OnsceneFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		currentScene = scene.buildIndex;
		if (currentScene == MultiplayerSetting.multiplayerSetting.multiplayerScene)
		{
			isGameLoaded = true;
			if (MultiplayerSetting.multiplayerSetting.delayStart)
			{
				PV.RPC("RPC_LoadedGameScene", RpcTarget.MasterClient);
			}
			else
			{
				RPC_CreatePlayer();
			}
			//CreatePlayer();
		}
	}

	[PunRPC]
	private void RPC_LoadedGameScene()
	{
		playerInGame++;
		if (playerInGame == PhotonNetwork.PlayerList.Length)
		{
			PV.RPC("RPC_CreatePlayer", RpcTarget.All);
		}
	}

	[PunRPC]
	private void RPC_CreatePlayer()
	{
		//creates player network controller but not  player character
		PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
	}

	public override void OnPlayerLeftRoom(Player otherPlayer)
	{
		base.OnPlayerLeftRoom(otherPlayer);
		Debug.Log(otherPlayer.NickName + "has left the game");
		playersInRoom--;
	}
	//private void CreatePlayer()
	//{
	//	PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PhotonNetworkPlayer"), transform.position, Quaternion.identity, 0);
	//}
}
