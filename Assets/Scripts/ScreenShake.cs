using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour{

    public static ScreenShake Instance {get; private set;}
    
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float shakeTimerMax;
    private float shakeTimer;
    private float startingIntensity;
    private float startingSpeed;

    private void Awake(){
        if(!Instance){
            Instance = this;
        }
    }

    public void ShakeCamera(float intensity, float speed, float time){
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
            cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        cinemachineBasicMultiChannelPerlin.m_FrequencyGain = speed;

        startingIntensity = intensity;
        startingSpeed = speed;

        shakeTimerMax = time;
        shakeTimer = time;
    }

    private void Update(){
        if(shakeTimer >= 0){
            shakeTimer -= Time.deltaTime;

            if(shakeTimer <= 0){
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = 
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, 1- (shakeTimer/shakeTimerMax));
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(startingSpeed, 0f, 1- (shakeTimer/shakeTimerMax));
            }
        }
    }
}
