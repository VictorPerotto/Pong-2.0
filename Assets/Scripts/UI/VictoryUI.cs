using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
    public static VictoryUI Instance;

    [SerializeField] private TextMeshProUGUI victoryText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button restartButton;

    private void Awake(){

        if(Instance == null){
            Instance = this;
        }

        restartButton.onClick.AddListener(() => {
            GameManager.LocalInstance.RestartGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            GameManager.LocalInstance.GoToMainMenu();
        });

        Hide();
    }

    private void Start(){
        ScoreManager.OnAnyPlayerWins += ScoreManager_OnAnyPlayerWins;
    }

    private void ScoreManager_OnAnyPlayerWins(object sender, ScoreManager.OnAnyPlayerWinsEventArgs e){
        if(e.victoryPlayer == 1){
            Invoke("Player1Win", 15 * Time.fixedDeltaTime);
        } else {
            Invoke("Player2Win", 15 * Time.fixedDeltaTime);
        }
    }

    private void Player1Win(){
        victoryText.SetText("PLAYER 1 WINS");
        Show();
    }

    private void Player2Win(){
        victoryText.SetText("PLAYER 2 WINS");
        Show();
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
