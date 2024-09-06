using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreUI : MonoBehaviour{
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
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    private void Goal_OnAnyPlayerScored(object sender, Goal.OnPlayerScoredEventArgs e){
        if(e.playerOneScored){
            playerOneScoreText.SetText(ScoreManager.Instance.GetPlayerOneScore().ToString());
            playerOneAnimatorController.SetTrigger(SCORED);

        } else {
            playerTwoScoreText.SetText(ScoreManager.Instance.GetPlayerTwoScore().ToString());
            playerTwoAnimatorController.SetTrigger(SCORED);
        }
    }
}
