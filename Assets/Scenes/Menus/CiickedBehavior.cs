using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class ClickedBehavior : MonoBehaviour {

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
        string clickedButtonName = EventSystem.current.currentSelectedGameObject.name;
        if(clickedButtonName == "Register")
        {
            isRegister = true;
            register.onClick.AddListener(getUsernameInfoOnClick);
            register.onClick.AddListener(getPasswordInfoOnClick);
        }
        else
        {
            login.onClick.AddListener(getUsernameInfoOnClick);
            login.onClick.AddListener(getPasswordInfoOnClick);
        } 
    
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
