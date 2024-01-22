using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class LoginBehavior : MonoBehaviour {

    public Button login;

    public Button register;

    public bool isRegister;
    string username;
    string password;

    public InputField usernameIF;
    public InputField passwordIF;

    private void Start()
    {
        username = "";
        password = "";

        
        isRegister = false;
            register.onClick.AddListener(getUsernameInfoOnClick);
            register.onClick.AddListener(getPasswordInfoOnClick);
            login.onClick.AddListener(getUsernameInfoOnClick);
            login.onClick.AddListener(getPasswordInfoOnClick);

    
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
}
