using Steamworks;
using Steamworks.Data;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class LobbyMembersManager : MonoBehaviour
{
	//This is to control the lobby member list

	public FriendCard memberCardPrefab;

	public GameObject inviteFriendButton;
	public GameObject currentInviteFriendBtn;
	public GameObject leaveLobbyBtn;

	public GameObject lobbyMemderList;

	public List<FriendCard> membersInLobby = new();

	public static LobbyMembersManager instance;

	//Lol just a list to easliy hold and deleat gos in the list
	public List<GameObject> instincedStuff;

	private void Start()
	{
		if (instance)
			Destroy(this.gameObject);

		instance = this;

		SteamMatchmaking.OnLobbyMemberJoined += MemberJoinedLobby;

		SteamMatchmaking.OnLobbyMemberDisconnected += MemberLeftLobby;
		SteamMatchmaking.OnLobbyMemberLeave += MemberLeftLobby;

		//Update weather or not the player can leave the current lobby
		//We dont want a player to leave their own lobby as we want everyone to have their own, if they are in someone elses lobby then they can leave
		if (LobbyManager.Instance.currentLobby.IsOwnedBy(SteamClient.SteamId))
			leaveLobbyBtn.SetActive(false);
		else
			leaveLobbyBtn.SetActive(true);
	}

	//If a member leaves the lobby then we want to update the visuals
	private void MemberLeftLobby(Lobby lobby, Friend friend)
	{
		Debug.Log($"Member left lobby");
		UpdateMembersInLobbyList();
	}

	//If a member joins the lobby then we want to update the visuals
	private void MemberJoinedLobby(Lobby lobby, Friend friend)
	{
		Debug.Log($"Member joined lobby");
		UpdateMembersInLobbyList();
	}

	//Here we just remove all itemns on the visual list and re create them as it will not happen all that offten
	public async void UpdateMembersInLobbyList()
	{
		//Destroy all items
		foreach (var obj in instincedStuff)
		{
			Destroy(obj.gameObject);
		}

		//Destroy the invite byn
		Destroy(currentInviteFriendBtn);

		//Create a member card foir each member in lobby including us
		foreach (var member in LobbyManager.Instance.currentLobby.Members)
		{
			FriendCard newFriend = Instantiate(memberCardPrefab, lobbyMemderList.transform);
			newFriend.memberSteamID = member.Id;
			newFriend.memberName = member.Name;

			var image = await SteamFriends.GetLargeAvatarAsync(member.Id);
			newFriend.memberAvatar.texture = SteamTools.GetTexture2DFromImage(image.Value);

			membersInLobby.Add(newFriend);
			instincedStuff.Add(newFriend.gameObject);
		}

		//create an invite card to show the friends list
		currentInviteFriendBtn = Instantiate(inviteFriendButton, lobbyMemderList.transform);

		//Update the leave lobby btn as we may have joined a new lobby that we don own
		if (LobbyManager.Instance.currentLobby.IsOwnedBy(SteamClient.SteamId))
			leaveLobbyBtn.SetActive(false);
		else
			leaveLobbyBtn.SetActive(true);
	}
}