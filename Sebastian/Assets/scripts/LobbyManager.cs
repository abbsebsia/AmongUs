using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Relay;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay.Models;
using Unity.Networking.Transport.Relay;
using UnityEngine;
using Unity.Netcode;
using IngameDebugConsole;
using Unity.Netcode.Transports.UTP;

public class LobbyManager : MonoBehaviour
{

	private static Lobby hostLobby;
	private float hearbeatTimer;

	private static string playerName; 

	private async void Start()
	{
		try
		{
			await UnityServices.InitializeAsync();
			Debug.Log("Initialize Services completed");

			AuthenticationService.Instance.SignedIn += () =>
			{
				Debug.Log("Signed in " + AuthenticationService.Instance.PlayerId);
			};

			await AuthenticationService.Instance.SignInAnonymouslyAsync();
			playerName = "seb" + UnityEngine.Random.Range(1, 99);
			Debug.Log("player name: " + playerName);
		}
		catch (LobbyServiceException e)
		{
			Debug.LogException(e);
		}
	}


	[ConsoleMethod("createRelay", "create Relay")]
	public async static void CreateRelay()
    {
        try
        {
			Allocation allocation = await RelayService.Instance.CreateAllocationAsync(4);

			string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

			Debug.Log("joincode " + joinCode);

			RelayServerData relayServerData = new RelayServerData(allocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

			NetworkManager.Singleton.StartHost();
        }catch(RelayServiceException e)
        {
			Debug.LogError(e);
        }
    }

	[ConsoleMethod("joinRelay", "join Relay")]
	public async static void JoinRelay(string joinCode)
    {
        try
        {
			Debug.Log("Joining  relay with code:" + joinCode);

			JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

			RelayServerData relayServerData = new RelayServerData(joinAllocation, "dtls");
			NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(relayServerData);

			NetworkManager.Singleton.StartClient();
		}
		catch(RelayServiceException e)
        {
			Debug.Log(e);
        }
		
    }
	private async void Update()
    {
		if (hostLobby == null) return;
		hearbeatTimer -= Time.deltaTime;
		if(hearbeatTimer < 0f)
        {
			float heartbeatTimerMax = 15;
			hearbeatTimer = heartbeatTimerMax;

			Debug.Log("Pinged Lobby with Id: " +  hostLobby.Id);
			await LobbyService.Instance.SendHeartbeatPingAsync(hostLobby.Id);
        }
    }


	[ConsoleMethod("listLobbies", "listLobbies")]
	public async static void ListLobbies()
	{
		try
		{
			QueryResponse queryResponse = await Lobbies.Instance.QueryLobbiesAsync();

			Debug.Log("Lobbies found : " + queryResponse.Results.Count);
			foreach (Lobby lobby in queryResponse.Results)
			{
				Debug.Log(lobby.Name + " " + lobby.MaxPlayers);
			}
		}
		catch (LobbyServiceException e)
		{
			Debug.LogError(e);
		}


	}
	private void HandleLobbyPollForUpdates()
    {

    }
	private static  Player GetPlayer()
	{
		return new Player
		{
			Data = new Dictionary<string, PlayerDataObject>
					{
						{"PlayerName", new PlayerDataObject(PlayerDataObject.VisibilityOptions.Member, playerName) }
					}
		};

	}

	[ConsoleMethod("createLobby", "creates a lobby")]
	public async static void CreateLobby()
	{
		string lobbyName = "MyLobby";
		int maxPlayers = 4;
		try
		{
			CreateLobbyOptions createLobbyOptions = new CreateLobbyOptions
			{
				IsPrivate = false,
				Player = GetPlayer(),
			};

			Lobby myLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, maxPlayers , createLobbyOptions);

            hostLobby = myLobby;
			Debug.Log("Created Lobby " + myLobby.Name + " " + myLobby.MaxPlayers + " " + myLobby.Id + " " + myLobby.LobbyCode);

			PrintPlayers(hostLobby);
		}
		catch (LobbyServiceException e)
		{
			Debug.LogError(e);
		}

	}

	[ConsoleMethod("joinLobby", "join lobby")]
    public async static void JoinLobbyByCode(string lobbyCode)
    {
		try
		{
			JoinLobbyByCodeOptions joinLobbyByCodeOptions = new JoinLobbyByCodeOptions
			{
				Player = GetPlayer(),
			};
			Lobby joinedLobby = await Lobbies.Instance.JoinLobbyByCodeAsync(lobbyCode, joinLobbyByCodeOptions);
			Debug.Log("joined lobby with : " + lobbyCode);

			PrintPlayers(joinedLobby);
		}
		catch (LobbyServiceException e)
		{
			Debug.LogError(e);
		}

		
    }

	public static void PrintPlayers(Lobby lobby)
    {
		Debug.Log("PLayers in lobby :" + lobby.Name);
		foreach(Player player in lobby.Players)
        {
			Debug.Log(player.Data["PlayerName"].Value);
        }
    }

	
}
