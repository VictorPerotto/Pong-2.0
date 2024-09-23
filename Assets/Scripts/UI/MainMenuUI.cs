using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour{
    private const string LOBBY_SCENE = "LobbyScene";
    
    [SerializeField] private Button multiPlayerButton;

    private void Awake(){
        multiPlayerButton.onClick.AddListener(() => {
            SceneManager.LoadScene(LOBBY_SCENE);
        });
    }
}
