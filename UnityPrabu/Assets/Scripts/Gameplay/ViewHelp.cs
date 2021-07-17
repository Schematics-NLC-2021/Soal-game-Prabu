using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ViewHelp : MonoBehaviour
{
    public void ChangeToScene(int sceneToChangeTo)
    {
        // Application.LoadLevel(sceneToChangeTo); <- is obsolete
        SceneManager.LoadScene(sceneToChangeTo);
    }
}
