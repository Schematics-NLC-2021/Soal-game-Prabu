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
    public static float elapsedTime = 0f;

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

        StartCoroutine(UpdateTimer());
    }

    public void EndTimer()
    {
        timerGoing = false;    
    }

    private IEnumerator UpdateTimer()
    {
        float max_time = 603f;
        while(timerGoing && elapsedTime < max_time)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(max_time - elapsedTime);
            string timePlayingStr = string.Format("Timer:\n{0}", timePlaying.ToString("mm':'ss"));
            timerText.text = timePlayingStr;

            yield return null;
        }
        //if da loop is done, then it's end game moite, back to mainScene
        FindObjectOfType<ViewHelp>().ChangeToScene(0);
        FindObjectOfType<EndGameManager>().EndGame();
    }
}
