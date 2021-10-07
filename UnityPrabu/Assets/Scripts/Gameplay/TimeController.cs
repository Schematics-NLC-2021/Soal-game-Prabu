using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeController : MonoBehaviour
{
    public static TimeController instance;
    public Text timerText;
    private TimeSpan timePlaying;
    private bool timerGoing;
    // public static float elapsedTime = 0f;
    public static DateTime start_time = DateTime.Now;
    public static DateTime end_time = start_time.AddMinutes(10.05f);
    public static double diff_seconds = 3f;

    private void Awake()
    {
        instance = this;
        // if(instance == null)
        // {
        //     instance = this;
        //     DontDestroyOnLoad(this.gameObject);
        //     return;
        // }
        // Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        timerText.text = "Timer:\n10:00";
        timerGoing = false; 
        BeginTimer();  
    }

    public void BeginTimer()
    {
        timerGoing = true;
        // elapsedTime = 0f;
        // start_time = DateTime.Now;
        // end_time = start_time.AddMinutes(10);
        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;    
    }

    private IEnumerator UpdateTimer()
    {
        while(timerGoing && diff_seconds > 0)
        {
            DateTime now = DateTime.Now;
            diff_seconds = (end_time - now).TotalSeconds;
            int minutes = (int)Math.Floor(diff_seconds/60);
            string menit = (minutes < 10)? '0'+ minutes.ToString() : minutes.ToString();
            int seconds = (int)(diff_seconds % 60);
            string detik = (seconds < 10)? '0'+ seconds.ToString() : seconds.ToString();
            string timePlayingStr = string.Format("Timer:\n{0}: {1}", menit, detik);
            timerText.text = timePlayingStr;

            yield return null;
        }
        //if da loop is done, then it's end game moite, back to mainScene
        FindObjectOfType<ViewHelp>().ChangeToScene(0);
        FindObjectOfType<EndGameManager>().EndGame();
    }
}
