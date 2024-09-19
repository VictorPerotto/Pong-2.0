using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour{

    public static ScoreManager Instance {get; private set;}
    
    [SerializeField] private int scoreToFinish;
    private NetworkVariable<int> playerOneScore = new NetworkVariable<int>();
    private NetworkVariable<int> playerTwoScore = new NetworkVariable<int>();

    public static event EventHandler<OnScoreChangedEventArgs> OnPlayer1Scored;
    public static event EventHandler<OnScoreChangedEventArgs> OnPlayer2Scored;

    public class OnScoreChangedEventArgs : EventArgs{
        public int score;
    }

    public override void OnNetworkSpawn(){
        playerOneScore.OnValueChanged += OnPlayer1ScoredMethod;
        playerTwoScore.OnValueChanged += OnPlayer2ScoredMethod;
    }

    private void OnPlayer1ScoredMethod(int oldValue, int newValue){
        OnPlayer1Scored?.Invoke(this, new OnScoreChangedEventArgs {score = playerOneScore.Value});
    }

    private void OnPlayer2ScoredMethod(int oldValue, int newValue){
        OnPlayer2Scored?.Invoke(this, new OnScoreChangedEventArgs {score = playerTwoScore.Value});
    }

    private void Start(){
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    private void Goal_OnAnyPlayerScored(object sender, Goal.OnPlayerScoredEventArgs e){
        if(IsServer){
            if(e.playerOneScored){
                playerOneScore.Value ++;
            } else {
                playerTwoScore.Value ++;
            }

            CheckScore();
        }
    }

    private void CheckScore(){
        if(playerOneScore.Value >= scoreToFinish || playerTwoScore.Value >= scoreToFinish){
            GameManager.Instance.StartEndGameSequence();
        }
    }
}
