using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour{
    private const string LOCAL_GAME_SCENE = "LocalGameScene";
    
    [SerializeField] private Button singlePlayerButton;
    [SerializeField] private Button localMultiplayerButton;
    [SerializeField] private Button onlineMultiplayerButton;

    private void Awake(){
        singlePlayerButton.onClick.AddListener(() => {
            SceneManager.LoadScene(LOCAL_GAME_SCENE);
            SavedDataManager.isPlayerController = false;
        });

        localMultiplayerButton.onClick.AddListener(() => {
            SceneManager.LoadScene(LOCAL_GAME_SCENE);
            SavedDataManager.isPlayerController = true;
        });

        onlineMultiplayerButton.onClick.AddListener(() => {

        });
    }
}
