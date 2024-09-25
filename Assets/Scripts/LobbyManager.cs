using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour{
    
    public static LobbyManager Instance;
    
    private const string GAME_SCENE = "GameScene";
    private Lobby joinedLobby;

    private void Awake(){
        Instance = this;

        DontDestroyOnLoad(gameObject);

        InitializeUnityAuthentication();
    }

    private async void InitializeUnityAuthentication(){
        if(UnityServices.State != ServicesInitializationState.Initialized){
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(Random.Range(0, 1000).ToString());

            await UnityServices.InitializeAsync();

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateLobby(string lobbyName){
        try {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 2);

            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE, LoadSceneMode.Single);

        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void JoinLobby(string lobbyCode){
        try{
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            NetworkManager.Singleton.StartClient();
        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void DeleteLobby(){
        if(joinedLobby != null){
            try{
                await LobbyService.Instance.DeleteLobbyAsync(joinedLobby.Id);

                joinedLobby = null;
            } catch (LobbyServiceException e){
                Debug.Log(e);
            }
        }
    }

    public async void LeaveLobby(){
        if(joinedLobby != null){
            try{
                await LobbyService.Instance.RemovePlayerAsync(joinedLobby.Id, AuthenticationService.Instance.PlayerId);

                joinedLobby = null;
            } catch (LobbyServiceException e){
                Debug.Log(e);
            }
        }
    }

    public Lobby GetLobby(){
        return joinedLobby;
    }
}
