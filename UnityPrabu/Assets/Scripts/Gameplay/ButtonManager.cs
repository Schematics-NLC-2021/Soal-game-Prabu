using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//to import js  
// using System.Runtime.InteropServices;

public class ButtonManager : MonoBehaviour
{
    // [DllImport("__Internal")]
    // private static extern bool confirmActionfunc();
    private bool greenLight = false;
    public void SubmitFunc(){
        // Debug.Log("Mengsubmit");
        if(!ScoreManager.gameDone)
        {
            // Debug.Log("Mengkonferm");
            // bool greenLight = confirmActionfunc();
            // Debug.Log("No popu?");
            // return;
            if(greenLight){
                FindObjectOfType<EndGameManager>().EndGame();
            }
        }
            
    }
    public void MengPop()
    {
        if(!ScoreManager.gameDone && !PopUpSystem.isOnPop)
        {
            PopUpSystem pop = GameObject.FindGameObjectWithTag("GameManager").GetComponent<PopUpSystem>();
            pop.PopUp("Apakah anda yakin akan mengsubmit?");
        }
    }

    public void Yes()
    {
        greenLight = true;
        PopUpSystem.isOnPop = false;
        SubmitFunc();
    }

    public void No()
    {
        greenLight = false;
        PopUpSystem.isOnPop = false;
        SubmitFunc();
    }
}
