using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour{

    public static GameManager Instance {get; private set;}

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
    
    private void TogglePauseGame(){
        gameIsPaused = !gameIsPaused;

        if(gameIsPaused){
            Time.timeScale = 0;
            OnGamePaused?.Invoke(this, EventArgs.Empty);
        } else {
            Time.timeScale = 1f;
            OnGameUnpaused?.Invoke(this, EventArgs.Empty);
        }
    }
}
