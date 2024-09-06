using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour{

    public static ScoreManager Instance {get; private set;}
    
    [SerializeField] private int scoreToFinish;
    private int playerOneScore;
    private int playerTwoScore;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    private void Start(){
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    private void Goal_OnAnyPlayerScored(object sender, Goal.OnPlayerScoredEventArgs e){
        if(e.playerOneScored){
            playerOneScore ++;
        } else {
            playerTwoScore ++;
        }

        CheckScore();
    }

    private void CheckScore(){
        if(playerOneScore >= scoreToFinish){
            GameManager.Instance.StartEndGameSequence();
        } else if(playerTwoScore >= scoreToFinish){
            GameManager.Instance.StartEndGameSequence();
        }
    }

    public int GetPlayerOneScore(){
        return playerOneScore;
    }

    public int GetPlayerTwoScore(){
        return playerTwoScore;
    }
}
