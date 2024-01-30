using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization;
using UnityEngine.Networking;
using TMPro;


public class LoginFunctionality : MonoBehaviour
{

    public void LoadBack()
    {
        SceneManager.LoadScene("Map");
        AudioListener.volume = 1;
    }

    public TMP_InputField username;
    public TMP_InputField password;

    public void LoginButton()
    {
        Login(username.text, password.text);
    }

    public void CreateAccountButton()
    {
        CreateAccount(username.text, password.text);

        LoadBack();
    }

    [Serializable]
    public struct UserCredentials
    {
        public string username;
        public string password;
    }

    public void CreateAccount(string sUN, string sPW)
    {
        UserCredentials objUserData = new UserCredentials();
        objUserData.username = sUN;
        objUserData.password = sPW;
        string jsonUserData = JsonUtility.ToJson(objUserData);
        Debug.Log(jsonUserData);
        StartCoroutine(PostFileFromServer("createAccount", jsonUserData, CreateAccountCallback));
    }

    // sData is user counting id
    void CreateAccountCallback(string sData)
    {
        if (null == sData)
        {
            Debug.Log("Server error.");
            return;
        }
        else if (-1 == (int.Parse(sData)))
        {
            Debug.Log("Username taken");
        }
        else
        {
            PlayerPrefs.SetInt("UserID", int.Parse(sData));
            Debug.Log(sData);
            LoadBack();
        }
    }

    public void Login(string sUN, string sPW)
    {
        UserCredentials objUserData = new UserCredentials();
        objUserData.username = sUN;
        objUserData.password = sPW;
        string jsonUserData = JsonUtility.ToJson(objUserData);
        StartCoroutine(PostFileFromServer("login", jsonUserData, LoginCallback));
    }

    // sData is user counting id
    void LoginCallback(string sData)
    {
        if (null == sData)
        {
            Debug.Log("Server error.");
            return;
        }
        else if (-1 == (int.Parse(sData)))
        {
            Debug.Log("Username or password incorrect");
        }
        else
        {
            PlayerPrefs.SetInt("UserID", int.Parse(sData));
            Debug.Log(sData);
            LoadBack();
        }
    }

    IEnumerator PostFileFromServer(string sField, string jsonData, System.Action<string> doneCallback)
    {
        WWWForm form = new WWWForm();
        form.AddField(sField, jsonData);
        string sURL = "https://hwgo.codehw.net/main.php";
        using (UnityWebRequest webRequest = UnityWebRequest.Post(sURL, form))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
                doneCallback?.Invoke(null);
            }
            else
                doneCallback?.Invoke(webRequest.downloadHandler.text);
        }
    }
}