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

    public void Show(string victoryText){
        this.victoryText.SetText(victoryText);
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide(){
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
