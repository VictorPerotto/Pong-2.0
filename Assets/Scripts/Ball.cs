using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public static event EventHandler OnAnyCollision;

    public static void ResetStaticData(){
        OnAnyCollision = null;
    }

    public event EventHandler OnBallRestart;
    public event EventHandler<OnBallCollisionEventArgs> OnBallCollision;
    public class OnBallCollisionEventArgs : EventArgs{
        public Vector2 collisionPoint;
        public Vector2 moveDirection;
    }

    private const string LAUNCH_BALL_STRING = "LaunchBall";

    [SerializeField] private float speed;
    [SerializeField] private float incrementSpeedMultiplier;
    private float hitCounter;
    private Vector2 moveDirection;
    private Vector3 initialPosition;
    private Rigidbody2D rb;

    private void Awake(){
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        Invoke(LAUNCH_BALL_STRING, 2f);
    }

    private void FixedUpdate(){
        float baseMultiplier = 1;
        float moveX = moveDirection.x * speed * (baseMultiplier + hitCounter);
        float moveY = moveDirection.y * speed * (baseMultiplier + hitCounter);

        rb.velocity = new Vector2(moveX, moveY);
    } 
    
    private void LaunchBall(){
        OnBallRestart?.Invoke(this, EventArgs.Empty);

        int randomX = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        int randomY = UnityEngine.Random.Range(-1, 2);
        moveDirection = new Vector2 (randomX, randomY);
    }

    private void PlayerBounceBall(Transform playerTransform){

        float moveX;
        float moveY;

        if(transform.position.x < playerTransform.position.x ){
            moveX = -1;
        } else {
            moveX = 1;
        }

        if(transform.position.y > playerTransform.position.y){
            moveY = 1;
        } else {
            moveY = -1;
        }
         
        moveDirection = new Vector2(moveX, moveY);
        hitCounter += incrementSpeedMultiplier;
    }

    private void WallBounceBall(Transform wallTransform){
        float moveY;

        if(transform.position.y < wallTransform.position.y){
            moveY = -1;
        } else {
            moveY = 1;
        }

        moveDirection = new Vector2(moveDirection.x, moveY);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        OnAnyCollision?.Invoke(this, EventArgs.Empty);
        
        OnBallCollision?.Invoke(this, new OnBallCollisionEventArgs {
            collisionPoint = collision.GetContact(0).point,
            moveDirection = this.moveDirection
        });

        if(collision.gameObject.TryGetComponent<Player>(out Player player) ||
        collision.gameObject.TryGetComponent<PlayerTwo>(out PlayerTwo playerTwo)){
            PlayerBounceBall(collision.transform);
        } else {
            WallBounceBall(collision.transform);
        }
    }

    public void RestartBall(){
        moveDirection = Vector2.zero;
        rb.velocity = Vector2.zero;
        transform.position = initialPosition;
        hitCounter = 0;

        Invoke(LAUNCH_BALL_STRING, 3f);        
    }
}
