using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchScene : MonoBehaviour{
    
    private const string LOBBY_SCENE = "LobbyScene";

    private void LateUpdate(){
        SceneManager.LoadScene(LOBBY_SCENE);    
    }
}
