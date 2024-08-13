using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour{

    private const string PLAYER_AXIS_RAW_VERTICAL = "Vertical";

    private Rigidbody2D rb;

    [SerializeField] private float speed;
    private Vector2 moveDirection;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update(){
        GetMoveDirection();
    }

    private void FixedUpdate(){
        Move();
    }

    private void GetMoveDirection(){
        moveDirection = new Vector2(0, Input.GetAxisRaw(PLAYER_AXIS_RAW_VERTICAL));
    }

    private void Move(){
        rb.velocity = moveDirection * speed;
    }

}
