using Steamworks;
using Steamworks.Data;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
	//This is the lobby manager for creating joining and leaving lobbys

	//The current lobby we are in
	public Lobby currentLobby;

	public bool inLobby;

	public static LobbyManager Instance;

	//These are objs that we want to control at the start as if we are not in a lobby or steam hasent started we dont want their funtions to run
	public GameObject lobbyMembers;
	public GameObject steamFriends;
	public GameObject invitationManager;

	public int maxMembersOfLobby;

	private void Start()
	{
		if (Instance)
			Destroy(this);

		Instance = this;

		//Start of lobby life cycle
		SteamMatchmaking.OnLobbyCreated += OnLobbyCreated;
		SteamMatchmaking.OnLobbyInvite += OnLobbyInvite;
		SteamMatchmaking.OnLobbyEntered += OnLobbyEntered;

		SteamMatchmaking.CreateLobbyAsync(maxMembersOfLobby);
	}

	//Start of lobby life cycle
	private void OnLobbyCreated(Result result, Lobby lobby)
	{
		//Set up the first lobby currectly and enable the other objs
		currentLobby = lobby;
		currentLobby.SetJoinable(true);
		currentLobby.SetPublic();
		currentLobby.SetFriendsOnly();

		inLobby = true;

		lobbyMembers.SetActive(true);
		steamFriends.SetActive(true);
		invitationManager.SetActive(true);

		Debug.Log($"Lobby created, Name: {lobby.Owner.Name}, {result.ToString()} ({this.name})");
	}

	//When someone invites us we create an invitation in the top left, the invitation care is a member card that we change to work
	private void OnLobbyInvite(Friend friend, Lobby lobby)
	{
		Debug.Log($"Invite resieved from {friend.Name} to {lobby.Owner.Name} Lobby ({this.name})");

		InvitationsManager.instance.CreateInvitation(friend, lobby);
	}

	//When we enter a lobby we want to update the lobby member list and we want to conect to that lobbys asoiated owner
	private void OnLobbyEntered(Lobby lobby)
	{
		Debug.Log($"Entered lobby, Name: {lobby.Owner.Name} ({this.name})");

		LobbyMembersManager.instance.UpdateMembersInLobbyList();
		SteamP2PManager.instance.JoinNewP2PSesion(currentLobby);
	}


	//Custom lobby functions

	//Join a lobby and make sure it is valid
	public void JoinLobby(Lobby lobby)
	{
		if(lobby.Id == 0)
		{
			Debug.LogError($"Cant join lobby, it is invalid ({this.name})");
			return;
		}

		Debug.Log($"Trying to join lobby ({this.name})");
		currentLobby.Leave();
		currentLobby = lobby;
		currentLobby.Join();
	}

	//Leave a lobby and make sure its not owned by us
	public void LeaveLobby(bool dontRejoin = false)
	{
		if (!currentLobby.IsOwnedBy(SteamClient.SteamId) && !dontRejoin)
		{
			Debug.Log($"Trying to leave lobby ({this.name})");
			currentLobby.Leave();
		}
		else
		{
			Debug.Log($"Trying to leave lobby ({this.name})");
			currentLobby.Leave();
		}
	}
}
