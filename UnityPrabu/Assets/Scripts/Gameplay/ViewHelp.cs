using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewHelp : MonoBehaviour
{
    public static bool onMainScene = true;
    public void ChangeToScene(int sceneToChangeTo)
    {
        // Application.LoadLevel(sceneToChangeTo); <- is obsolete
        onMainScene = sceneToChangeTo == 0;
        //cek gamenya done or no
        if(!ScoreManager.gameDone && !PopUpSystem.isOnPop)
            //SceneManager is not from us, it's from SceneManagement, I've changed the SceneManager gameObject to Lolz, and it stil works
            SceneManager.LoadScene(sceneToChangeTo);
    }
}
