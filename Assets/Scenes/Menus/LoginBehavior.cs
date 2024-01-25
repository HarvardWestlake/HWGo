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


public class LoginBehavior : MonoBehaviour
{

    public Button login;

    public Button register;

    public bool isRegister;
    string username;
    string password;

    public TMP_InputField usernameIF;
    public TMP_InputField passwordIF;

    private void Start()
    {
        username = "";
        password = "";


        isRegister = false;
        register.onClick.AddListener(getUsernameInfoOnClick);
        register.onClick.AddListener(getPasswordInfoOnClick);
        login.onClick.AddListener(getUsernameInfoOnClick);
        login.onClick.AddListener(getPasswordInfoOnClick);

        CreateAccount(username, password);
    }

    public void getUsernameInfoOnClick()
    {
        username = usernameIF.text;
        Debug.Log(username);
    }

    public void getPasswordInfoOnClick()
    {
        password = passwordIF.text;
        Debug.Log(password);
    }

         [Serializable]

    public struct UserCredentials {

        public string username;

        public string password;

    }

 

    void CreateAccount(string sUN, string sPW) {

        UserCredentials objUserData = new UserCredentials();

        objUserData.username = sUN;

        objUserData.password = sPW;

        string jsonUserData = JsonUtility.ToJson(objUserData);

        Debug.Log(jsonUserData);

        StartCoroutine(PostFileFromServer("createAccount", jsonUserData, CreateAccountCallback));

    }

 

    // sData is user counting id

    void CreateAccountCallback(string sData) {

        if (null == sData) {

            Debug.Log("Server error.");

            return;

        }

        Debug.Log(sData);

    }

 

    IEnumerator PostFileFromServer(string sField, string jsonData, System.Action<string> doneCallback) {

        WWWForm form = new WWWForm();

        form.AddField(sField, jsonData);

        string sURL = "https://hwgo.codehw.net/main.php";

        using (UnityWebRequest webRequest = UnityWebRequest.Post(sURL, form)) {

            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError) {

                Debug.LogError("Error: " + webRequest.error);

                doneCallback?.Invoke(null);

            }

            else

                doneCallback?.Invoke(webRequest.downloadHandler.text);

        }

    }
}
