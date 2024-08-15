using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public static event EventHandler<OnPlayerScoredEventArgs> OnAnyPlayerScored;

    public class OnPlayerScoredEventArgs : EventArgs{
        public bool playerOneScored;
    }

    public static void ResetStaticData(){
        OnAnyPlayerScored = null;
    }

    [SerializeField] private bool playerOneGoal;

    private void OnTriggerEnter2D(Collider2D collider2D){
        if(playerOneGoal){
            OnAnyPlayerScored?.Invoke(this, new OnPlayerScoredEventArgs{playerOneScored = false});
        } else {
            OnAnyPlayerScored?.Invoke(this, new OnPlayerScoredEventArgs{playerOneScored = true});
        }

        collider2D.gameObject.GetComponent<Ball>().RestartBall();
    }
}
