using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCard : MonoBehaviour
{
	//This script is only for the local client however, it is now not in use

	[SerializeField] private TMP_Text playerNameTxt;

	public string playerName;
	public RawImage playerAvatar;

	private async void Start()
	{
		playerName = SteamClient.Name;
		playerNameTxt.text = playerName;

		var image = await SteamFriends.GetLargeAvatarAsync(SteamClient.SteamId);

		playerAvatar.texture = SteamTools.GetTexture2DFromImage(image.Value);
	}
}
