using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text scoreText;

    public void ShowResult(string msg){
        // Debug.Log(msg);
        scoreText.text = msg;
    }
}
