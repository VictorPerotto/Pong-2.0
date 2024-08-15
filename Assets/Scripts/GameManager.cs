using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour{

    public static GameManager Instance {get; private set;}

    private const string MAIN_MENU_SCENE_NAME = "MainMenuScene";

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;

    private bool gameIsPaused;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)){
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
        Time.timeScale = 1f;
    }

    public void GoToMainMenu(){
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME);
        Time.timeScale = 1f;
    }
}
