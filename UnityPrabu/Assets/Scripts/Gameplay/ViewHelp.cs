using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewHelp : MonoBehaviour
{
    public void ChangeToHelp(int sceneToChangeTo)
    {
        // Application.LoadLevel(sceneToChangeTo); <- is obsolete
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
