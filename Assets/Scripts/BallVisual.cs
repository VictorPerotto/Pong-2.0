using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class BallVisual : NetworkBehaviour{

    private const string BOUNCE_BALL_ANIMATION = "BallBounce";
    
    [SerializeField] private Material ballMaterial;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private ParticleSystem bounceParticles;
    [SerializeField] private ParticleSystem goalParticles;
    
    private Animator ballAnimatorController;

    private void Awake(){
        ballAnimatorController = GetComponent<Animator>();
    }

    private void Start(){
        Ball.OnAnyCollision += Ball_OnBallCollision;
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    private void Goal_OnAnyPlayerScored(object sender, Goal.OnPlayerScoredEventArgs e){
        SpawnGoalParticlesServerRpc();
    }
 
    private void Ball_OnBallCollision(object sender, Ball.OnBallCollisionEventArgs e){
        SpawnBounceParticlesServerRpc(e.collisionPoint, -e.moveDirection);
    }

    private void SpawnParticles(ParticleSystem particles,Vector2 spawnPosition, Vector2 spawnDirection){
        Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, spawnDirection);

        Instantiate(particles, spawnPosition, spawnRotation);
    }

    [ServerRpc()]
    private void SpawnGoalParticlesServerRpc(){
        SpawnGoalParticlesClientRpc();
    }

    [ClientRpc]
    private void SpawnGoalParticlesClientRpc(){
        SpawnParticles(goalParticles, gameObject.transform.position, new Vector2(0,0));
    }

    [ServerRpc(RequireOwnership = false)]
    private void SpawnBounceParticlesServerRpc(Vector2 collisionPoint, Vector2 moveDirection){
        SpawnBounceParticlesClientRpc(collisionPoint, moveDirection);
    }

    [ClientRpc]
    private void SpawnBounceParticlesClientRpc(Vector2 collisionPoint, Vector2 moveDirection){
        SpawnParticles(bounceParticles, collisionPoint, moveDirection);
        ScreenShake.Instance.ShakeCamera(.3f, 5f, .2f);
        ballAnimatorController.SetTrigger(BOUNCE_BALL_ANIMATION);
    }
}
