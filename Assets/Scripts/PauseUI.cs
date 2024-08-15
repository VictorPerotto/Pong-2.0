using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour{
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake(){
        resumeButton.onClick.AddListener(() => {
            GameManager.Instance.TogglePauseGame();
        });

        restartButton.onClick.AddListener(() => {
            GameManager.Instance.RestartGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            GameManager.Instance.GoToMainMenu();
        });
    }

    private void Start(){
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused ;
        GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    }

    private void GameManager_OnGamePaused(object sender, EventArgs e){
        Show();
    }
    
    private void GameManager_OnGameUnpaused(object sender, EventArgs e){
        Hide();
    }

    private void Show(){
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void Hide(){
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
