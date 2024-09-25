using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : NetworkBehaviour{

    private const string PLAYER_ONE_AXIS_RAW_VERTICAL = "VerticalOne";

    private Rigidbody2D rb;

    [SerializeField] private float speed;
    [SerializeField] private List<Vector3> spawnPositions;
    private Vector2 moveDirection;

    private void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnNetworkSpawn(){
        if(IsHost){
            transform.position = spawnPositions[0];
        } else {
            transform.position = spawnPositions[1];
        }   
    }

    private void Update(){
        if(IsOwner){
            GetMoveDirection();
        }
    }

    private void FixedUpdate(){
        Move();
    }

    private void GetMoveDirection(){
        moveDirection = new Vector2(0, Input.GetAxisRaw(PLAYER_ONE_AXIS_RAW_VERTICAL));
    }

    private void Move(){
        rb.velocity = moveDirection * speed;
    }

}
