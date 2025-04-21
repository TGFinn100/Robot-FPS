using Netcode.Transports.Facepunch;
using Steamworks;
using System;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpManager : NetworkBehaviour
{
	//This script managers all things that we want to be set up befor the user can interact with the main menu
	//Steam, authentication

	[SerializeField] private string playerID;
	[SerializeField] private string playerToken;

	[SerializeField] private string playerSteamName;
	public string playerSteamID;

	[SerializeField] private string mainMenuSceneName;

	public static StartUpManager Instance;

	public bool isSteamInited;

	private void Awake()
	{
		//Create an instace and dont destroy this
		if (Instance != null)
			Destroy(this.gameObject);

		Instance = this;

		DontDestroyOnLoad(this.gameObject);
	}

	private void Start()
	{
		//NetworkManager.OnClientStarted += OnClientStarted;

		Init();
	}

	private void OnClientStarted()
	{
		Debug.Log($"Host Running");
	}

	private async void Init()
	{
		//Init the unity services
		await UnityServices.InitializeAsync();
		if (UnityServices.State == ServicesInitializationState.Initialized)
		{
			Debug.Log("Unity services has intialised!");
			LogIn();
		}
		if (UnityServices.State == ServicesInitializationState.Uninitialized)
			Debug.Log("Unity services has failed to intialised!");
	}

	private async void LogIn()
	{
		//Sign in to unity
		try
		{
			await AuthenticationService.Instance.SignInAnonymouslyAsync();
		}
		catch (Exception e)
		{
			Debug.Log(e.ToString());
			throw;
		}

		//Conferm we loged in and save sone key info
		if (AuthenticationService.Instance.IsSignedIn)
		{
			Debug.Log("Unity Authentication has loged in");
			playerID = AuthenticationService.Instance.PlayerId;
			playerToken = AuthenticationService.Instance.AccessToken;
			StartSteam();
		}
		else
			Debug.Log("Unity Authentication failed to log in");
	}

	private void StartSteam()
	{
		//Init facepunch steam
		try
		{
			SteamClient.Init(480, true);
		}
		catch (Exception exc)
		{
			Debug.LogError("Error starting steam Maybe: Steam is closed or Can't find steam_api dlls or Don't have permission to open appid");
		}

		//Confem we inited and save some key info
		if (SteamClient.IsLoggedOn)
		{
			playerSteamName = SteamClient.Name;
			playerSteamID = SteamClient.SteamId.ToString();
			Debug.Log($"Steam Is running, Name: {playerSteamName} ID: {playerSteamID}");
			isSteamInited = true;

			//Load in to the main menu as everyting is set up for the user to interact
			SceneManager.LoadScene(mainMenuSceneName);
		}
		else
		{
			Debug.LogError("Error not logged in");
		}
	}
}
