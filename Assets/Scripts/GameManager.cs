using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO.IsolatedStorage;

public class GameManager : MonoBehaviour{

    public static GameManager Instance {get; private set;}

    private const string MAIN_MENU_SCENE_NAME = "MainMenuScene";

    public static event EventHandler OnAnyPlayerWin; 

    public static void ResetStaticData(){
        OnAnyPlayerWin = null;
    }

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
    [SerializeField] private float slowMotionDurationMax = 2f;
    private float slowMotionDuration;
    private GameEndState gameEndState;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }

        slowMotionDuration = slowMotionDurationMax;
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

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1f;
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
        Time.timeScale = 1f;
    }

    private void EndGameSequence(){
        switch(gameEndState){
            case GameEndState.SlowMotion:
                if(slowMotionDuration > 0){
                    slowMotionDuration -= Time.fixedDeltaTime;
                    Time.timeScale = slowMotionFactor;
                } else {
                    OnAnyPlayerWin?.Invoke(this, EventArgs.Empty);
                    slowMotionDuration = slowMotionDurationMax;
                    gameEndState = GameEndState.VictoryScreen;
                }
            break;

            case GameEndState.VictoryScreen:
                isGameEnding = false;
                Time.timeScale = 0;
            break;
        }
    }

    public void StartEndGameSequence(){
        gameEndState = GameEndState.SlowMotion;
        isGameEnding = true;
    }
}
