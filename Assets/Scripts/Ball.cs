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

    [SerializeField] private float incrementSpeedMultiplier;
    [SerializeField] private float initialSpeed;
    [SerializeField] private float maxSpeed;
    private Vector3 initialPosition;
    private Vector2 moveDirection;
    private float hitCounter;
    private float speed;

    private Rigidbody2D rb;
    
    private void Awake(){
        initialPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        Invoke(LAUNCH_BALL_STRING, 2f);
    }

    private void FixedUpdate(){
        
    } 
    
    private void LaunchBall(){
        OnBallRestart?.Invoke(this, EventArgs.Empty);

        int randomX = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
        int randomY = UnityEngine.Random.Range(-1, 2);

        MoveBall(new Vector2(randomX, randomY));
    }

    private void MoveBall(Vector2 direction){
        moveDirection = direction.normalized;

        speed = initialSpeed + hitCounter * incrementSpeedMultiplier;

        rb.velocity = moveDirection * speed;
    }

    private void PlayerBounceBall(Collision2D collision){
        float moveX;
        if(transform.position.x < collision.transform.position.x ){
            moveX = -1;
        } else {
            moveX = 1;
        }

        Vector3 ballPosition = transform.position;
        Vector3 paddlePosition = collision.transform.position;
        float paddleHeight = collision.collider.bounds.size.y;
 
        float moveY = (ballPosition.y - paddlePosition.y)/ paddleHeight;

        moveDirection = new Vector2(moveX, moveY);

        if(speed < maxSpeed){
            hitCounter ++;
        }
        
        MoveBall(moveDirection);
    }

    private void OnCollisionEnter2D(Collision2D collision){
        OnAnyCollision?.Invoke(this, EventArgs.Empty);
        
        OnBallCollision?.Invoke(this, new OnBallCollisionEventArgs {
            collisionPoint = collision.GetContact(0).point,
            moveDirection = this.moveDirection
        });

        if(collision.gameObject.TryGetComponent<Player>(out Player player) ||
        collision.gameObject.TryGetComponent<PlayerTwo>(out PlayerTwo playerTwo)){
            PlayerBounceBall(collision);
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
