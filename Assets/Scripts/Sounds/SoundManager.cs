using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour{

    public static SoundManager Instance;

    private float volume = 1f;

    [SerializeField] private AudioClip bounceBall;
    [SerializeField] private AudioClip victory;
    [SerializeField] private AudioClip goal;

    private void Awake(){
        if(Instance == null){
            Instance = this;
        }
    }

    private void Start(){
        Ball.OnAnyCollision += Ball_OnAnyCollision;
        Goal.OnAnyPlayerScored += Goal_OnAnyPlayerScored;
    }

    public void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f){
        AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
    }

    private void Ball_OnAnyCollision(object sender, EventArgs e){
        PlaySound(bounceBall, Camera.main.transform.position , volume);
    }

    private void Goal_OnAnyPlayerScored(object sender, EventArgs e){
        PlaySound(goal, Camera.main.transform.position , volume);
    }
}
