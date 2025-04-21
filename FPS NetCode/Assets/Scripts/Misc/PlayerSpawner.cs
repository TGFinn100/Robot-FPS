using System;
using Unity.Netcode;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawner : NetworkBehaviour
{
	public GameObject playerPrefab;
	private string scene = "MainGame";
	private bool isStarted;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	private void OnLoadComplete(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
	{
		Debug.Log($"Load Complete ({this.name})");
		Spawn_ServerRpc();
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();

		Debug.Log($"NetworkSpawned ({this.name})");
		SceneManager.sceneLoaded += SceneManager_sceneLoaded;
	}

	private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
	{
		Debug.Log($"Load Complete ({this.name})");
		Spawn_ServerRpc();
	}

	[ServerRpc(RequireOwnership = false)]
	private void Spawn_ServerRpc(ServerRpcParams rpcParams = default)
	{
		ulong senderClientId = rpcParams.Receive.SenderClientId;

		Spawn_ByServer(senderClientId);
	}

	void Spawn_ByServer(ulong conn)
	{
		// Instantiate player object
		NetworkPrefabsList networkPrefabsList = Resources.Load<NetworkPrefabsList>("NetworkPrefabsList");


		GameObject player = Instantiate(playerPrefab);

		// Get the NetworkObject component
		var playerNetworkObject = player.GetComponent<NetworkObject>();

		// Spawn the player object on all clients
		playerNetworkObject.SpawnAsPlayerObject(conn);
	}
}
