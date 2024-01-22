using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public AudioClip admin, art, english, fight, grass, history, language, maintenance, math, pairot;
    public string sceneName;
    private static SceneManage instance;
    public MusicTransition musicTransition;

    // * this is made by Ryan to test the music transitions.

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //swapping scenes for testing purposes
        if (Input.GetKeyDown("1"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Map");
        }
        if (Input.GetKeyDown("2"))
        {
            SceneManager.LoadScene("InventoryAll");
        }
        if (Input.GetKeyDown("3"))
        {
            SceneManager.LoadScene("TitleMenu");
        }
        if (Input.GetKeyDown("4"))
        {
            SceneManager.LoadScene("Login");
        }
    }
}
