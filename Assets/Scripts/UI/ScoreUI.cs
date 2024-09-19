using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreUI : NetworkBehaviour{
    private const string SCORED = "Scored";

    private Animator playerOneAnimatorController;
    private Animator playerTwoAnimatorController;

    [SerializeField] private TextMeshProUGUI playerOneScoreText;
    [SerializeField] private TextMeshProUGUI playerTwoScoreText;
    
    private void Awake(){
        playerOneAnimatorController = playerOneScoreText.gameObject.GetComponent<Animator>();
        playerTwoAnimatorController = playerTwoScoreText.gameObject.GetComponent<Animator>();
    }
    
    private void Start(){
        ScoreManager.OnPlayer1Scored += ScoreManager_OnPlayer1Scored;
        ScoreManager.OnPlayer2Scored += ScoreManager_OnPlayer2Scored;
    }

    private void ScoreManager_OnPlayer1Scored(object sender, ScoreManager.OnScoreChangedEventArgs e){
        playerOneScoreText.SetText(e.score.ToString());
        playerOneAnimatorController.SetTrigger(SCORED);
    }

    private void ScoreManager_OnPlayer2Scored(object sender, ScoreManager.OnScoreChangedEventArgs e){
        playerTwoScoreText.SetText(e.score.ToString());
        playerTwoAnimatorController.SetTrigger(SCORED);
    }
}
