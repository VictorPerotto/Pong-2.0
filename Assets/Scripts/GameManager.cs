using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO.IsolatedStorage;
using Unity.Netcode;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : NetworkBehaviour{
    private const string MAIN_MENU_SCENE_NAME = "MainMenuScene";

    public static event EventHandler OnAnyPlayerWins;
    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool gameIsPaused;
    private bool isGameEnding;

    private enum GameEndState{
        Playing,
        SlowMotion,
        VictoryScreen,
    }

    [SerializeField] private float slowMotionFactor = 0.2f;
    [SerializeField] private float slowMotionDurationMax = 1f;
    private float slowMotionDuration;
    private GameEndState gameEndState;

    [SerializeField] private NetworkObject ballPrefab;

    public static void ResetStaticData(){
        OnAnyPlayerWins = null;
    }

    private void Awake(){
        slowMotionDuration = slowMotionDurationMax;
    }

    private void Start(){
        NetworkManager.OnClientConnectedCallback += OnClientConnected;
        ScoreManager.OnAnyPlayerWins += ScoreManager_OnAnyPlayerWins;
    }

    private void OnClientConnected(ulong clientId){
        if(!IsServer){
            SpawnBall();
        }
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.P) && !isGameEnding){
            TogglePauseGame();
        }

        if(isGameEnding){
            EndGameSequence();
        }
    }
    
    public void TogglePauseGame(){
        gameIsPaused = !gameIsPaused;

        if(gameIsPaused){
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }

    public static void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public static void GoToMainMenu(){
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
        Time.timeScale = 1f;
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
}
