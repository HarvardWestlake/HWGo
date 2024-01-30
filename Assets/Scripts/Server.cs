using System;

using UnityEngine;

using UnityEngine.Networking;

using System.Collections;

using System.Runtime.Serialization;

 

public class Server: MonoBehaviour {

    // on click

    void Start() {

        Debug.Log("Program Starting");

        CreateAccount("Jake", "JakesPassword");

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