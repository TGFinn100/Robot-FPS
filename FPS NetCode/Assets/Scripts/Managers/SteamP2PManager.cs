using Netcode.Transports.Facepunch;
using Steamworks;
using Steamworks.Data;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamP2PManager : NetworkBehaviour
{
	//This script is a one stop shop for controlling the netcode server and client funtionality
	//If the client isint running it will check to see if the user is the owner of the lobby 
	//If they are the owner then we want to start as a host so otheres connect to this one
	//If not owner then we wana start client and connect to the owner

	public static SteamP2PManager instance;

	public Lobby lobbyToJoin;

	private void Start()
	{
		if (instance)
			Destroy(this);

		instance = this;

		NetworkManager.OnClientStopped += NetworkManager_OnClientStopped;
	}

	//Used to loop back around to keep the funtionality in this script
	private void NetworkManager_OnClientStopped(bool obj)
	{
		JoinNewP2PSesion(lobbyToJoin);
	}

	public void JoinNewP2PSesion(Lobby lobby)
	{
		lobbyToJoin = lobby;

		//If host or client, the client will be running
		if (NetworkManager.Singleton.IsClient)
		{
			//As we onlu call this to set up a new connection we will shut down the client and or server and circle back
			NetworkManager.Singleton.Shutdown();
		}

		//If client is not running and we own the lobby we will start as host
		if (!NetworkManager.Singleton.IsClient && lobby.IsOwnedBy(SteamClient.SteamId))
		{
			Debug.Log($"Starting Host ({this.name})");
			HandleHostTransport(lobbyToJoin.Owner.Id);
		}

		//If client is not running and we are not the owner we will start as client
		else if (!NetworkManager.Singleton.IsClient && !lobby.IsOwnedBy(SteamClient.SteamId))
		{
			Debug.Log($"Starting Client ({this.name})");
			HandleClientTransport(lobbyToJoin.Owner.Id);
		}
	}

	private void HandleHostTransport(SteamId id)
	{
		//Sets the steam transport ID to the OWNER'S ID OF THE LOBBY
		NetworkManager.Singleton.GetComponent<FacepunchTransport>().targetSteamId = id;
		NetworkManager.Singleton.StartHost();
	}

	private void HandleClientTransport(SteamId id)
	{
		//Sets the steam transport ID to the OWNER'S ID OF THE LOBBY
		NetworkManager.Singleton.GetComponent<FacepunchTransport>().targetSteamId = id;
		NetworkManager.Singleton.StartClient();
	}
}
