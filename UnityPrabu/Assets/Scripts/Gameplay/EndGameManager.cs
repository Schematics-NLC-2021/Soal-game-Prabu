using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//to import js  
using System.Runtime.InteropServices;
//for text ui
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    public Text doneText;
    public Image bgDone;
    [DllImport("__Internal")]
    private static extern void sendScore(int score);
    
    void Start(){
        //entah kenapa dia ke start lagi
        if(!ScoreManager.gameDone)
            bgDone.enabled = false;
    }

    public void EndGame(){
        FindObjectOfType<TimeController>().EndTimer();
        ScoreManager.gameDone = true;
        int score = FindObjectOfType<GridManager>().CalculateScore();
        var scoreMsg = string.Format("Your Score:\n{0} out of 30", score);
        FindObjectOfType<ScoreManager>().ShowResult(scoreMsg);
        sendScore(score);
        bgDone.enabled = true;
        doneText.text = "Silakan kembali ke game utama";   
    }
}
