using UnityEngine.SceneManagement;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.IO.IsolatedStorage;

public class GameManager : MonoBehaviour{

    public static GameManager Instance {get; private set;}

    private const string MAIN_MENU_SCENE_NAME = "MainMenuScene";

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool gameIsPaused;
    private bool onVictoryScreen;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape) && !onVictoryScreen){
            TogglePauseGame();
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
        onVictoryScreen = false;
        Time.timeScale = 1f;
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
        onVictoryScreen = false;
        Time.timeScale = 1f;
    }

    public void FinishGame(string victoryText){
        VictoryUI.Instance.Show(victoryText);
        onVictoryScreen = true;
        Time.timeScale = 0;
    }
}
