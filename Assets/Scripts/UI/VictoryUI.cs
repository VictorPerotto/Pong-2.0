using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
            GameManager.Instance.RestartGame();
        });

        mainMenuButton.onClick.AddListener(() => {
            GameManager.Instance.GoToMainMenu();
        });

        Hide();
    }

    private void Start(){
        GameManager.OnAnyPlayerWin += GameManager_OnAnyPlayerWin;
    }

    private void GameManager_OnAnyPlayerWin(object sender, EventArgs e){
        Debug.Log("Victory UI");
        if(ScoreManager.Instance.GetPlayerOneScore() > ScoreManager.Instance.GetPlayerTwoScore()){
            Show("PLAYER 1 WINS");
        } else {
            Show("PLAYER 2 WINS");
        }
    }

    private void Show(string victoryText){
        this.victoryText.SetText(victoryText);
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
