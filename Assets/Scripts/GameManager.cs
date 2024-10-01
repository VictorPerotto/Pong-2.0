using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO.IsolatedStorage;
using Unity.Netcode;
using UnityEngine.SocialPlatforms.Impl;
using System.Collections.Generic;
using Unity.Services.Lobbies.Models;

public class GameManager : NetworkBehaviour{

    public static GameManager LocalInstance;

    private const string LOBBY_SCENE_GAME = "LobbyScene";

    public static event EventHandler OnAnyPlayerWins;

    private bool gameIsPaused;
    private bool isGameEnding;

    private enum GameEndState{
        Playing,
        SlowMotion,
        VictoryScreen,
    }

    [SerializeField] private Transform playerPrefab;
    [SerializeField] private float slowMotionFactor = 0.2f;
    [SerializeField] private float slowMotionDurationMax = 1f;
    private float slowMotionDuration;
    private GameEndState gameEndState;

    [SerializeField] private NetworkObject ballPrefab;

    public static void ResetStaticData(){
        OnAnyPlayerWins = null;
    }

    private void Awake(){
        if(LocalInstance == null){
            LocalInstance = this;
        } else {
            Destroy(LocalInstance);
        }

        slowMotionDuration = slowMotionDurationMax;
    }

    public override void OnNetworkSpawn(){
        ScoreManager.OnAnyPlayerWins += ScoreManager_OnAnyPlayerWins;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        if(IsHost){
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.Singleton.LocalClientId, true);
        }
    }

    private void OnClientConnected(ulong clientId){
        if(IsHost){
            Transform playerTransform = Instantiate(playerPrefab);
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }

        if(!IsHost){
            SpawnBall();
        }
    }

    private void Update(){
        if(isGameEnding){
            EndGameSequence();
        }
    }

    public void GoToMainMenu(){
        if(IsHost){
            LobbyManager.Instance.DeleteLobby();
            
            SceneManager.LoadScene(LOBBY_SCENE_GAME, LoadSceneMode.Single);
            
            NetworkManager.Singleton.Shutdown();
        } 
        
        if(IsClient) {
            LobbyManager.Instance.LeaveLobby();

            SceneManager.LoadScene(LOBBY_SCENE_GAME, LoadSceneMode.Single);
            
            NetworkManager.Singleton.Shutdown();
        }
    }

    private void ScoreManager_OnAnyPlayerWins(object sender, ScoreManager.OnAnyPlayerWinsEventArgs e){
        StardEndGameSequenceClientRpc();
    }

    [ClientRpc]
    private void StardEndGameSequenceClientRpc(){
        gameEndState = GameEndState.SlowMotion;
        isGameEnding = true;
    }

    private void EndGameSequence(){
        switch(gameEndState){
            case GameEndState.SlowMotion:
                if(slowMotionDuration > 0){
                    slowMotionDuration -= Time.fixedDeltaTime;
                    Time.timeScale = slowMotionFactor;
                } else {
                    slowMotionDuration = slowMotionDurationMax;
                    gameEndState = GameEndState.VictoryScreen;
                }
            break;

            case GameEndState.VictoryScreen:
                isGameEnding = false;
                OnAnyPlayerWins?.Invoke(this, EventArgs.Empty);
                Time.timeScale = 0;
            break;
        }
    }

    public void SpawnBall(){
        SpawnBallServerRpc();
        Debug.Log("Spawn");
    }

    [ServerRpc(RequireOwnership = false)]
    public void SpawnBallServerRpc(){
        GameObject instantiadeBall = Instantiate(ballPrefab.gameObject, Vector3.zero, Quaternion.identity);
        instantiadeBall.GetComponent<NetworkObject>().Spawn(true);
    }

    public override void OnDestroy(){
        ScoreManager.OnAnyPlayerWins -= ScoreManager_OnAnyPlayerWins;
        NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
    }
}
