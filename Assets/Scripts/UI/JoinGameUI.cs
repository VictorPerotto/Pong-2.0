using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JoinGameUI : MonoBehaviour{

    public static JoinGameUI Instance;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TMP_InputField lobbyCodeField;
    [SerializeField] private Button join;
    [SerializeField] private Button closeButton;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }

        Hide();

        join.onClick.AddListener(() => LobbyManager.Instance.JoinLobby(lobbyCodeField.text));

        closeButton.onClick.AddListener(Hide);
    }

    public void Show(){
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    public void Hide(){
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
