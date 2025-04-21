using Steamworks;
using Steamworks.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class SteamFriendsManager : MonoBehaviour
{
	//This script manages the steam friends list

	//Lists containinmg friend s deppending on their online status
	public List<FriendStruct> friendsPlayingThis = new();
	public List<FriendStruct> onlineFriends = new();
	public List<FriendStruct> offlineFriends = new();
	public FriendStruct player;

	public static SteamFriendsManager instance;

	//transforms in the ui structure
	public GameObject playingThisTransform;
	public GameObject onlineTransform;
	public GameObject offlineTransform;

	public FriendCard friendCardPrefab;

	public GameObject view;

	private async void Start()
	{
		if (instance)
			Destroy(this);

		instance = this;

		//Get all the steam friends
		foreach (var friend in SteamFriends.GetFriends())
		{
			//Create a friend struct for each friend
			FriendStruct newFriend = new FriendStruct();

			var image = await SteamFriends.GetLargeAvatarAsync(friend.Id);
			newFriend.friendAvatar = SteamTools.GetTexture2DFromImage(image.Value);
			newFriend.friendName = friend.Name;
			newFriend.friendSteamID = friend.Id;

			if (friend.IsOnline)
			{
				if (friend.IsMe)
					player = newFriend;
				else
				{
					if (friend.IsPlayingThisGame)
					{
						friendsPlayingThis.Add(newFriend);
					}
					else
					{
						onlineFriends.Add(newFriend);
					}
				}
			}
			else
			{
				offlineFriends.Add(newFriend);
			}
		}

		CreatFriendsList();
	}

	//Creates the visual list of friends
	private void CreatFriendsList()
	{
		foreach (var friend in friendsPlayingThis)
		{
			FriendCard newFriendCard = Instantiate(friendCardPrefab, playingThisTransform.transform);
			newFriendCard.memberAvatar.texture = friend.friendAvatar;
			newFriendCard.memberName = friend.friendName;
			newFriendCard.memberSteamID = friend.friendSteamID;
			newFriendCard.transform.localPosition = new Vector3(0, playingThisTransform.transform.localScale.y * 0.5f, 0);
			newFriendCard.inviteBtn.SetActive(true);
		}

		onlineTransform = Instantiate(onlineTransform, playingThisTransform.transform);

		foreach (var friend in onlineFriends)
		{
			FriendCard newFriendCard = Instantiate(friendCardPrefab, onlineTransform.transform);
			newFriendCard.memberAvatar.texture = friend.friendAvatar;
			newFriendCard.memberName = friend.friendName;
			newFriendCard.memberSteamID = friend.friendSteamID;
			newFriendCard.transform.localPosition = new Vector3(0, playingThisTransform.transform.localScale.y * 0.5f, 0);
			newFriendCard.inviteBtn.SetActive(true);
		}

		offlineTransform = Instantiate(offlineTransform, onlineTransform.transform);

		foreach (var friend in offlineFriends)
		{
			FriendCard newFriendCard = Instantiate(friendCardPrefab, offlineTransform.transform);
			newFriendCard.memberAvatar.texture = friend.friendAvatar;
			newFriendCard.memberName = friend.friendName;
			newFriendCard.memberSteamID = friend.friendSteamID;
			newFriendCard.transform.localPosition = new Vector3(0, playingThisTransform.transform.localScale.y * 0.5f, 0);
		}
	}
}

//A struct i currently use to hold friend info, i might just use steam friends struct in stead as this semms like im making an extra step
//One thing i would like to know if a friend that was saved half an houre ago is an instance of that time i downloaded it or if its updating
[System.Serializable]
public struct FriendStruct
{
	public Texture2D friendAvatar;
	public string friendName;
	public SteamId friendSteamID;

	public override string ToString()
	{
		return $"{friendAvatar},{friendName},{friendSteamID}";
	}
}
