using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyUI : MonoBehaviour{

    private const string GAME_SCENE = "GameScene";
    
    [SerializeField] private Button CreateGameButton;
    [SerializeField] private Button JoinGameButton;

    private void Awake(){
        CreateGameButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene(GAME_SCENE, LoadSceneMode.Single);
        });

        JoinGameButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
        });
    }
}
