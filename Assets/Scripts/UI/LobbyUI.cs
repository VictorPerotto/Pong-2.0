using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour{

    [SerializeField] private Button CreateGameButton;
    [SerializeField] private Button JoinGameButton;

    private void Start(){
        CreateGameButton.onClick.AddListener(CreateLobby);

        JoinGameButton.onClick.AddListener(() => {
            JoinGameUI.Instance.Show();
        });
    }

    private void CreateLobby(){
        LobbyManager.Instance.CreateLobby(Random.Range(0, 99).ToString());
    }
}
