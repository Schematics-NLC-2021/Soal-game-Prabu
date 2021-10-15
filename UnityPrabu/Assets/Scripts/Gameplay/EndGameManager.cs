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
    public Text timerText;
    [DllImport("__Internal")]
    private static extern void sendScore(int score);
    
    void Start(){
        //entah kenapa dia ke start lagi
        if(!ScoreManager.gameDone)
            bgDone.enabled = false;
    }

    public void EndGame(){
        //stop the timer
        FindObjectOfType<TimeController>().EndTimer();
        //stop the game from updating
        ScoreManager.gameDone = true;
        //calculate the score
        int score = FindObjectOfType<GridManager>().CalculateScore();
        //create text score string
        timerText.text = "";
        var scoreMsg = string.Format("Your Score:\n{0} out of 30", score);
        //show the score
        FindObjectOfType<ScoreManager>().ShowResult(scoreMsg);
        //send to server
        sendScore(score);
        //show border silakan kembali
        bgDone.enabled = true;
        //show pesan silakan kembali
        doneText.text = "Silakan kembali ke game utama";   
    }
}
