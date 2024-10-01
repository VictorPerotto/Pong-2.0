using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour{

    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;
    [SerializeField] private Button quitButton;

    private void Start(){
        createGameButton.onClick.AddListener(CreateLobby);

        joinGameButton.onClick.AddListener(() => {
            JoinGameUI.Instance.Show();
        });

        quitButton.onClick.AddListener(() => Application.Quit());
    }

    private void CreateLobby(){
        LobbyManager.Instance.CreateLobby(Random.Range(0, 99).ToString());
    }
}
