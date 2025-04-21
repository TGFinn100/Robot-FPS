using Steamworks;
using Steamworks.Data;
using System.Collections.Generic;
using UnityEngine;

public class InvitationsManager : MonoBehaviour
{
	//This script manages all of the inivitation functionality

	private List<GameObject> invitations = new();
	public FriendCard friendCardPrefab;

	[Tooltip("The amount of thime the invitation will live befor being destroyed")]
	public float invitationLifeTime;

	public static InvitationsManager instance;

	private void Start()
	{
		if (instance)
			Destroy(this.gameObject);

		instance = this;
	}

	//When an invitation to a lobby is called in the lobby manager, it calls this and one is created and it is destroyed after so many seconds
	public async void CreateInvitation(Friend friend, Lobby lobby)
	{
		FriendCard newInvitation = Instantiate(friendCardPrefab, transform);
		newInvitation.memberName = friend.Name;

		var image = await SteamFriends.GetLargeAvatarAsync(friend.Id);
		newInvitation.memberAvatar.texture = SteamTools.GetTexture2DFromImage(image.Value);
		newInvitation.memberLobby = lobby;
		newInvitation.joinBtn.SetActive(true);
		Destroy(newInvitation.gameObject, invitationLifeTime);
	}
}
