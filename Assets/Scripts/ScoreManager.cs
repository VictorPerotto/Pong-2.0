using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ScoreManager : NetworkBehaviour{

    public static ScoreManager Instance {get; private set;}
    
    [SerializeField] private int scoreToFinish;
    private int playerOneScore;
    private int playerTwoScore;

    public static event EventHandler<OnScoreChangedEventArgs> OnPlayerOneScored;
    public static event EventHandler<OnScoreChangedEventArgs> OnPlayerTwoScored;

    public static event EventHandler<OnAnyPlayerWinsEventArgs> OnAnyPlayerWins;

    public class OnAnyPlayerWinsEventArgs : EventArgs{
        public int victoryPlayer;
    }

    public class OnScoreChangedEventArgs : EventArgs{
        public int score;
    }

    public static void ResetStaticData(){
        OnPlayerOneScored = null;
        OnPlayerTwoScored = null;
        OnAnyPlayerWins = null;
    }

    [ClientRpc]
    private void OnPlayerOneScoredClientRpc(){
        playerOneScore ++;
        OnPlayerOneScored?.Invoke(this, new OnScoreChangedEventArgs {score = playerOneScore});
    }

    [ClientRpc]
    private void OnPlayerTwoScoredClientRpc(){
        playerTwoScore ++;
        OnPlayerTwoScored?.Invoke(this, new OnScoreChangedEventArgs {score = playerTwoScore});
    }

    private void Start(){
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    private void Goal_OnAnyPlayerScored(object sender, Goal.OnPlayerScoredEventArgs e){
        if(IsServer){
            if(e.playerOneScored){
                OnPlayerOneScoredClientRpc();
            } else {
                OnPlayerTwoScoredClientRpc();
            }
        }

        CheckScoreClientRpc();
    }
    
    [ClientRpc]
    private void CheckScoreClientRpc(){
        if(playerOneScore >= scoreToFinish){
            OnAnyPlayerWins?.Invoke(this, new OnAnyPlayerWinsEventArgs {victoryPlayer = 1});
        } else if(playerTwoScore >= scoreToFinish) {
            OnAnyPlayerWins?.Invoke(this, new OnAnyPlayerWinsEventArgs {victoryPlayer = 2});
        }
    }
}
