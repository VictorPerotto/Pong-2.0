using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour{

    [SerializeField] private float speed;
    [SerializeField] private Ball ball;
    [SerializeField] private float deadZone;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        float moveY;

        if(transform.position.y + deadZone > ball.transform.position.y ){
            moveY = -1;    
        } else if(transform.position.y - deadZone < ball.transform.position.y) {
            moveY = 1;
        } else {
            moveY = 0;
        }

        moveDirection = new Vector2 (0, moveY);
    }

    private void FixedUpdate(){
        rb.velocity = moveDirection * speed;
    }
}
