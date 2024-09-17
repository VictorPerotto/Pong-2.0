using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : NetworkBehaviour
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
        if(IsServer){
            if(playerOneGoal){
                OnAnyPlayerScored?.Invoke(this, new OnPlayerScoredEventArgs{playerOneScored = false});
            } else {
                OnAnyPlayerScored?.Invoke(this, new OnPlayerScoredEventArgs{playerOneScored = true});
            }
            collider2D.gameObject.GetComponent<Ball>().RestartBall();
        }
    }
}
