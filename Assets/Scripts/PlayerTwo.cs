using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwo : MonoBehaviour{

    private const string PLAYER_TWO_AXIS_RAW_VERTICAL = "VerticalTwo";
    [SerializeField] private bool isPlayerController;

    [SerializeField] private float speed;
    [SerializeField] private Ball ball;
    [SerializeField] private float deadZone;
    private Vector2 moveDirection;
    private Rigidbody2D rb;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start(){
        isPlayerController = SavedDataManager.isPlayerController;
    }

    private void Update(){
        if(isPlayerController){
            GetMoveDirection();
        } else {
            GetAIMoveDirection();
        }
    }

    private void FixedUpdate(){
        rb.velocity = moveDirection * speed;
    }

    private void GetAIMoveDirection(){
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

    private void GetMoveDirection(){
        moveDirection = new Vector2(0, Input.GetAxisRaw(PLAYER_TWO_AXIS_RAW_VERTICAL));
    }
}
