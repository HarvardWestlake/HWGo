using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour
{
    public void LoadSettingsScene()
    {
        SceneManager.LoadScene("SettingsGeneral");
    }
}
