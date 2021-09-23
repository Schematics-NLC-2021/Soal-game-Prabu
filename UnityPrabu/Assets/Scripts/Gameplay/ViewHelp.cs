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
        SceneManager.LoadScene(sceneToChangeTo);
        // deactivate all skrip
        // GameObject[] children = new GameObject[42];
        // for(int i = 0;i<42;i++){
        //     GameObject tempOb = Grids.transform.GetChild(i).gameObject;
        //     if(sceneToChangeTo != 0)
        //         tempOb.GetComponent<TilesManager>().onMainScene = false;
        //     else{
        //         tempOb.GetComponent<TilesManager>().onMainScene = false;
        //     }
        //     Debug.Log("Looking good");
        // }
    }
}
