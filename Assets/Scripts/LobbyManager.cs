using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Netcode;
using UnityEngine.SceneManagement;
using Unity.Services.Relay.Models;
using Unity.Services.Relay;
using System.Threading.Tasks;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;

public class LobbyManager : MonoBehaviour{
    
    public static LobbyManager Instance;
    
    private const string GAME_SCENE = "GameScene";
    private const string KEY_RELAY_JOIN_CODE = "RelayJoinCode";
    private Lobby joinedLobby;
    private float heartbeatTimer;

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

    private async Task<Allocation> AllocateRelay(){
        try{
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(2);

            return allocation;
        } catch (RelayServiceException e){
            Debug.Log(e);

            return default;
        }
    }

    private async Task<string> GetRelayJoinCode(Allocation allocation){
        try{
            string relayJoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

            return relayJoinCode;
        } catch (RelayServiceException e){
            Debug.Log(e);

            return default;
        }
    }

    private async Task<JoinAllocation> JoinRelay(string joinCode){
        try{
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
        
            return joinAllocation;
        } catch (RelayServiceException e) {
            Debug.Log(e);

            return default; 
        }
    }

    private void Update(){
        HandleHeartbeat();
    }

    private void HandleHeartbeat(){
        if(IsLobbyHost()){
            heartbeatTimer -= Time.deltaTime;
            if(heartbeatTimer <= 0){
                float heartbeatTimerMax = 15f;
                heartbeatTimer = heartbeatTimerMax;

                LobbyService.Instance.SendHeartbeatPingAsync(joinedLobby.Id);
            }
        }
    }

    private bool IsLobbyHost(){
        return joinedLobby != null && joinedLobby.HostId == AuthenticationService.Instance.PlayerId;
    }

    public async void CreateLobby(string lobbyName){
        try {
            joinedLobby = await LobbyService.Instance.CreateLobbyAsync(lobbyName, 2);

            Allocation allocation = await AllocateRelay();

            string relayJoinCode = await GetRelayJoinCode(allocation);

            await LobbyService.Instance.UpdateLobbyAsync(joinedLobby.Id, new UpdateLobbyOptions{
                Data = new Dictionary<string, DataObject> {
                    {KEY_RELAY_JOIN_CODE, new DataObject(DataObject.VisibilityOptions.Member, relayJoinCode)}
                }
            });

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(allocation, "dtls"));

            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE, LoadSceneMode.Single);

        } catch (LobbyServiceException e){
            Debug.Log(e);
        }
    }

    public async void JoinLobby(string lobbyCode){
        try{
            joinedLobby = await LobbyService.Instance.JoinLobbyByCodeAsync(lobbyCode);

            string relayJoinCode = joinedLobby.Data[KEY_RELAY_JOIN_CODE].Value;

            JoinAllocation joinAllocation = await JoinRelay(relayJoinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetRelayServerData(new RelayServerData(joinAllocation, "dtls"));

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
