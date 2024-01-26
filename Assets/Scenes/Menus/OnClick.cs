using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OnClick : MonoBehaviour
{
    public void LoadBack()
    {
        SceneManager.LoadScene("Map");
        AudioListener.volume = 1;
    }
}
