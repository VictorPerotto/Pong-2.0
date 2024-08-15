using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour{

    [SerializeField] private TextMeshProUGUI playerOneScoreText;
    [SerializeField] private TextMeshProUGUI playerTwoScoreText;
    
    private void Start(){
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    private void Goal_OnAnyPlayerScored(object sender, Goal.OnPlayerScoredEventArgs e){
        if(e.playerOneScored){
            playerOneScoreText.SetText(ScoreManager.Instance.GetPlayerOneScore().ToString());

        } else {
            playerTwoScoreText.SetText(ScoreManager.Instance.GetPlayerTwoScore().ToString());
        }
    }
}
