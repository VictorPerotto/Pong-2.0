using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwo : MonoBehaviour{

    private const string PLAYER_TWO_AXIS_RAW_VERTICAL = "VerticalTwo";
    [SerializeField] private bool isPlayerController;

    [SerializeField] private float playerSpeed;
    [SerializeField] private float deadZone;
    [SerializeField] private float AISpeed;
    [SerializeField] private Ball ball;
    private float currentY;
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
            MoveAI();
        }
    }

    private void FixedUpdate(){
        rb.velocity = moveDirection * playerSpeed;
    }

    private void GetAIMoveDirection(){
        float targetY = ball.transform.position.y;
    
        if (Mathf.Abs(transform.position.y - targetY) <= deadZone){
            currentY = transform.position.y;  
            return;
        }
        
        if (transform.position.y + deadZone > targetY){
            currentY = Mathf.Lerp(transform.position.y, targetY - deadZone, Time.deltaTime * AISpeed);
        }
        
        else if (transform.position.y - deadZone < targetY){
            currentY = Mathf.Lerp(transform.position.y, targetY + deadZone, Time.deltaTime * AISpeed);
        }
    }

    private void MoveAI(){
        Vector2 targetPosition = new Vector2(transform.position.x, currentY);
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, AISpeed * Time.deltaTime);
    }

    private void GetMoveDirection(){
        moveDirection = new Vector2(0, Input.GetAxisRaw(PLAYER_TWO_AXIS_RAW_VERTICAL));
    }
}
