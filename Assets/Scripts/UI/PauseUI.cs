using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour{
    
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Button mainMenuButton;
    private bool isPaused;

    private void Awake(){
        mainMenuButton.onClick.AddListener(() => GameManager.LocalInstance.GoToMainMenu());
    }

    private void Update(){
        if(Input.GetKeyDown(KeyCode.P)){
            TogglePauseGame();
        }
    }

    public void TogglePauseGame(){
        if(isPaused){
            Hide();
            isPaused = false;
        } else {
            Show();
            isPaused = true;
        }
    }

    public void Hide(){
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void Show(){
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }
}
