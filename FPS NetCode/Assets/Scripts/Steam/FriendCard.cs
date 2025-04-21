using Steamworks;
using Steamworks.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendCard : MonoBehaviour
{
	//This is a scrip to hold steam friends info and allow the client to interact with friends

	public GameObject kickBtn;
	public GameObject inviteBtn;
	public GameObject joinBtn;

	[SerializeField] private TMP_Text memberNameTxt;

	public string memberName;
	public Lobby memberLobby;
	public SteamId memberSteamID;
	public RawImage memberAvatar;

	private void Start()
	{
		memberNameTxt.text = memberName;
	}

	public void InviteToLobby()
	{
		//Checks to see if the card was set up correcly
		if (memberSteamID == 0)
		{
			Debug.LogError($"Friend -{memberName}- steam ID is not set");
			return;
		}

		//Calls steam to invite the player to the lobby
		LobbyManager.Instance.currentLobby.InviteFriend(memberSteamID);
	}

	//Join button
	public void JoinLobby()
	{
		//Checks to see if the card was set up correcly
		if (memberLobby.Id == 0)
		{
			Debug.LogError($"Friend -{memberName}- Lobby is not set");
			return;
		}

		//Joins the lobby the plays sent with the invitation
		LobbyManager.Instance.JoinLobby(memberLobby);
	}

	//Kick button, not set up
	public void KickFromLobby()
	{
		Debug.LogWarning($"Not implemented, will use server to call rpc to do so on client being kicked");
	}
}
