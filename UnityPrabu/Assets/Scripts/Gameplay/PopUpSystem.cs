using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public Text popUpText;
    public static bool isOnPop = false;
    public void PopUp(string msg)
    {
        popUpBox.SetActive(true);
        popUpText.text = msg;
        animator.SetTrigger("NeedPop");
        isOnPop = true;
        
    }
}
