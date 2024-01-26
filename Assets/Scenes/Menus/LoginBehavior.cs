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

//USERNAME AND PASSWORD STRINGS ARENT ACC UPDATING CHANGE IT SO THAT THEY ACTUALLY CHANGE INTO THE REAL USERNAME AND PASSSWORD
//WAIT IM TYRING SOMETHING
    public Button login;

    public Button register;

    public bool isRegister;
    string username;
    string password;

    public TMP_InputField usernameIF;
    public TMP_InputField passwordIF;

    private void Start()
    {

        isRegister = true;
        username = register.onClick.AddListener(getUsernameInfoOnClick);
        password = register.onClick.AddListener(getPasswordInfoOnClick);
        username = login.onClick.AddListener(getUsernameInfoOnClick);
        password = login.onClick.AddListener(getPasswordInfoOnClick);

        Debug.Log(username);
        Debug.Log(password);

        CreateAccount(username, password);
    }

    public string getUsernameInfoOnClick()
    {
        username = usernameIF.text;
        Debug.Log(username);
        return username;
    }

    public string getPasswordInfoOnClick()
    {
        password = passwordIF.text;
        Debug.Log(password);

        return password;
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
