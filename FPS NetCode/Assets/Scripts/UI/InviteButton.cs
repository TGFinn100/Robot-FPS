using UnityEngine;

public class InviteButton : MonoBehaviour
{
    //This is used to show the button to invite a steam friend
    public void ToggleFriendList()
    {
        SteamFriendsManager.instance.view.SetActive(true);
    }
}
