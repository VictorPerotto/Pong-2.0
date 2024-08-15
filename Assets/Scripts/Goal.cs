using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public event EventHandler OnPlayerOneScored;
    public event EventHandler OnPlayerTwoScored;

    [SerializeField] private bool playerOneGoal;

    private void OnTriggerEnter2D(Collider2D collider2D){
        if(playerOneGoal){
            OnPlayerTwoScored?.Invoke(this, EventArgs.Empty);
        } else {
            OnPlayerOneScored?.Invoke(this, EventArgs.Empty);
        }

        collider2D.gameObject.GetComponent<Ball>().RestartBall();
    }
}
