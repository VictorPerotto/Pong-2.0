using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;

public class LobbyInfoUI : MonoBehaviour{
    
    [SerializeField] private TextMeshProUGUI lobbyCodeText;

    private void Start(){
        Lobby myLobby = LobbyManager.Instance.GetLobby();

        lobbyCodeText.SetText("LOBBY CODE: " + myLobby.LobbyCode.ToString());
    }
}
