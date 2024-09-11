using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallVisual : MonoBehaviour{

    private const string BOUNCE_BALL_ANIMATION = "BallBounce";
    
    [SerializeField] private Ball ball;
    [SerializeField] private Material ballMaterial;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem bounceParticles;
    [SerializeField] private ParticleSystem goalParticles;
    
    private Animator ballAnimatorController;

    private void Awake(){
        ballAnimatorController = GetComponent<Animator>();
    }

    private void Start(){
        ball.OnBallRestart += Ball_OnBallRestart;
        ball.OnBallCollision += Ball_OnBallCollision;
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    private void Ball_OnBallRestart(object sender, EventArgs e){
       gameObject.SetActive(true);
    }

    private void Goal_OnAnyPlayerScored(object sender, Goal.OnPlayerScoredEventArgs e){
        SpawnParticles(goalParticles, gameObject.transform.position, new Vector2(0,0));
        gameObject.SetActive(false);
    }

    private void Ball_OnBallCollision(object sender, Ball.OnBallCollisionEventArgs e){
        SpawnParticles(bounceParticles, e.collisionPoint, -e.moveDirection);
        ScreenShake.Instance.ShakeCamera(.3f, 5f, .2f);
        ballAnimatorController.SetTrigger(BOUNCE_BALL_ANIMATION);
    }

    private void SpawnParticles(ParticleSystem particles,Vector2 spawnPosition, Vector2 spawnDirection){
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, spawnDirection);

        Instantiate(particles, spawnPosition, spawnRotation);
    }
}
