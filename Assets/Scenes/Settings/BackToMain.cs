using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMain : MonoBehaviour
{
    public void LoadBack()
    {
        SceneManager.LoadScene("Map");
    }
}
