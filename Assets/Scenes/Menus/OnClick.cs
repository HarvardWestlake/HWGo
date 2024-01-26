using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using UnityEngine.Networking;

public class OnClick : MonoBehaviour
{
    public void LoadBack()
    {
        SceneManager.LoadScene("Map");
        AudioListener.volume = 1;
    }



}
