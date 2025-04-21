using Steamworks;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public string gameScene;

	public void Play()
	{
		if (LobbyManager.Instance.currentLobby.Id == 0)
			Debug.LogWarning("Cant start game as lobby is invalid");

		if(!LobbyManager.Instance.currentLobby.IsOwnedBy(SteamClient.SteamId))
			Debug.LogWarning("Cant start game as you are not the lobby owner");

		Debug.Log("Starting game, changing scene");
		SceneManager.LoadScene(gameScene);
	}

	public void LoadOut()
	{

	}

	public void Settings()
	{

	}

	//If in editor leave play mode else we are in stand alone so exit from lobby and exit game
	public void Quit()
	{
		#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
		#endif

		#if PLATFORM_STANDALONE_WIN
		Application.Quit();
		#endif

		LobbyManager.Instance.LeaveLobby(true);
		SteamClient.Shutdown();
	}
}
