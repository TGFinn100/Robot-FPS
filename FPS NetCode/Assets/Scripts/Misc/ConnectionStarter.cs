using Netcode.Transports.Facepunch;
//using ParrelSync;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;

[RequireComponent(typeof(UnityTransport))]
[RequireComponent(typeof(FacepunchTransport))]
public class ConnectionStarter : MonoBehaviour
{
	public NetworkTransport unityTransport;
	public NetworkTransport facePunchTransport;
	public NetworkManager networkManager;

	public NetworkPrefabs list;

	public bool startServer;

	private void Start()
	{
		if (LobbyManager.Instance)
		{
			Debug.Log("Destroying test networkmanager as lobbymanager is in scene");
			this.gameObject.SetActive(false);
		}
		else
		{
			if (startServer)
			{
				Debug.Log("Starting UDP as Host");
				var config = new NetworkConfig();
				config.NetworkTransport = unityTransport;
				config.Prefabs = list;
				NetworkManager.Singleton.NetworkConfig = config;
				NetworkManager.Singleton.StartHost();
			}
			else
			{
				Debug.Log("Starting UDP as Client");
				var config = new NetworkConfig();
				config.NetworkTransport = unityTransport;
				config.Prefabs = list;
				NetworkManager.Singleton.NetworkConfig = config;
				NetworkManager.Singleton.StartClient();
			}

			//if (ClonesManager.IsClone())
			//{
			//	Debug.Log("Starting UDP as clone");
			//	var config = new NetworkConfig();
			//	config.NetworkTransport = unityTransport;
			//	config.Prefabs = list;
			//	NetworkManager.Singleton.NetworkConfig = config;
			//	NetworkManager.Singleton.StartClient();
			//}
			//else
			//{
			//	Debug.Log("Starting UDP as Host");
			//	var config = new NetworkConfig();
			//	config.NetworkTransport = unityTransport;
			//	config.Prefabs = list;
			//	NetworkManager.Singleton.NetworkConfig = config;
			//	NetworkManager.Singleton.StartHost();
			//}
		}
	}
}
