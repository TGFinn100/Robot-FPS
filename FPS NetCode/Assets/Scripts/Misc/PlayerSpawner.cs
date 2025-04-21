using Unity.Netcode;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
	public GameObject playerPrefab;
	private string scene = "MainGame";
	private bool isStarted;

	private void Start()
	{
		//Spawn_ServerRpc();
	}

	private void OnConnectionEvent(NetworkManager arg1, ConnectionEventData arg2)
	{
		Spawn_ServerRpc();
	}

	public override void OnNetworkSpawn()
	{
		base.OnNetworkSpawn();
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
