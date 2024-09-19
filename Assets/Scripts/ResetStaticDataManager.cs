using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour{

    private void Awake(){
        Goal.ResetStaticData();
        Ball.ResetStaticData();
        ScoreManager.ResetStaticData();
        GameManager.ResetStaticData();
    }
}
