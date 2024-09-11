using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeScreenshot : MonoBehaviour{
    [SerializeField] private bool isEnabled;

    public static void Screenshot(){
        ScreenCapture.CaptureScreenshot("Screenshot.png");
    }

    private void Update(){
        if(isEnabled){
            if(Input.GetKeyDown(KeyCode.T)){
                Screenshot();
            }
        }
    }
}
