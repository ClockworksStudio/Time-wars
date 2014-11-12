using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class NetworkManager : MonoBehaviour {

	public GameObject playerPrefab;
	public string roomName = "Server Name:";
	public int maxPlayerNum = 5;

	private RoomInfo[] roomsList;

	// Use this for initialization
	void Start ()
	{
		PhotonNetwork.ConnectUsingSettings("0.0.2");
	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	void OnGUI()
	{
		if (!PhotonNetwork.connected)
		{
			GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		}
		else if (PhotonNetwork.room == null)
		{
			GUI.Label(new Rect(100, 10, 250, 20), "Server Name:");
			roomName = GUI.TextField(new Rect(100, 30, 250, 20), roomName, 20);
			//GUI.Label(new Rect(100, 50, 250, 20), "Max Players:");
			//maxPlayerNum = int.Parse(GUI.TextField(new Rect(100, 70, 250, 20), maxPlayerNum.ToString(), 2));
			//Regex.Replace(maxPlayerNum.ToString(), @"[^0-9 ]", "");


			// Create Room
			if (GUI.Button(new Rect(100, 100, 250, 100), "Start Server"))
			{
				RoomOptions roomOptions = new RoomOptions() { isOpen = true, isVisible = true, maxPlayers = maxPlayerNum};
				PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
			}
			
			// Join Room
			if (roomsList != null)
			{
				for (int i = 0; i < roomsList.Length; i++)
				{
					if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + roomsList[i].name))
					{
						PhotonNetwork.JoinRoom(roomsList[i].name);
					}
				}
			}
		}
	}
	
	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}
	void OnJoinedRoom()
	{
		Debug.Log("Connected to Room!");

		// Spawn player
		PhotonNetwork.Instantiate(playerPrefab.name, Vector3.up * 5, Quaternion.identity, 0);
	}
}
