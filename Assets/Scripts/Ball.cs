using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private float speed;
    private float incrementSpeedMultiplier;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        LaunchBall();
    }

    private void FixedUpdate(){
        float baseMultiplier = 1;
        float moveX = moveDirection.x * speed * (baseMultiplier + incrementSpeedMultiplier);
        float moveY = moveDirection.y * speed * (baseMultiplier + incrementSpeedMultiplier);

        rb.velocity = new Vector2(moveX, moveY);
    } 
    
    private void LaunchBall(){
        int randomX = Random.Range(0, 2) == 0 ? -1 : 1;
        int randomY = Random.Range(-1, 2);
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
        incrementSpeedMultiplier += .025f;
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
        if(collision.gameObject.TryGetComponent<Player>(out Player player) ||
        collision.gameObject.TryGetComponent<AIPlayer>(out AIPlayer AIplayer)){
            PlayerBounceBall(collision.transform);
        } else {
            WallBounceBall(collision.transform);
        }
    }
}
