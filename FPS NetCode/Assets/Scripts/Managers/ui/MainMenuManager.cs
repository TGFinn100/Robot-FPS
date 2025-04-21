using Steamworks;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : NetworkBehaviour
{
	public string gameScene;

	public void Play()
	{
		if (LobbyManager.Instance.currentLobby.Id == 0)
			Debug.LogWarning($"Cant start game as lobby is invalid ({this.name})");

		if(!LobbyManager.Instance.currentLobby.IsOwnedBy(SteamClient.SteamId))
			Debug.LogWarning($"Cant start game as you are not the lobby owner ({this.name})");
		//SceneManager.LoadScene(gameScene);
		//NetworkManager.Singleton.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);



		if (IsHost && !string.IsNullOrEmpty(gameScene))
		{
			Debug.Log($"Starting game, changing scene ({this.name})");
			var status = NetworkManager.SceneManager.LoadScene(gameScene, LoadSceneMode.Single);
			if (status != SceneEventProgressStatus.Started)
			{
				Debug.LogWarning($"Failed to load {gameScene} " +
					  $"with a {nameof(SceneEventProgressStatus)}: {status} ({this.name})");
			}
		}
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
