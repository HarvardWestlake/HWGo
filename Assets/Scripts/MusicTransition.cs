using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicTransition : MonoBehaviour
{
    public AudioListener thisListener;
    public AudioClip admin, art, english, fight, grass, history, language, maintenance, math, pairot;
    public AudioSource track01, track02;
    private static MusicTransition instance;
    private bool isPlayingTrack01 = true;

    void Awake() {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwapTrack(AudioClip newClip)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTrack(newClip));
        //if (isPlayingTrack01)
        //{
        //    track02.clip = newClip;
        //    track02.Play();
        //    track01.Stop();
        //} else
        //{
        //    track01.clip = newClip;
        //    track01.Play();
        //    track02.Stop();
        //}

        isPlayingTrack01 = !isPlayingTrack01;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (FindObjectsOfType<AudioListener>().Length == 1)
        //{
        //    thisListener.enabled = true;
        //}
        //else
        //{
        //    thisListener.enabled = false;
        //}

        if (scene.name == "Map")
        {
            SwapTrack(art);
        }
        else if (scene.name == "TitleMenu")
        {
            SwapTrack(pairot);
        }
        else if (scene.name == "CatchProfessor")
        {
            SwapTrack(math);
        } else if (scene.name == "SettingsGeneral")
        {
            SwapTrack(maintenance);
        }
        else if (scene.name == "InventoryAll")
        {
            SwapTrack(english);
        }
        else if (scene.name == "Login")
        {
            SwapTrack(language);
        }
    }
    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float timeToFade = 1.5f;
        float timeElapsed = 0;

        if (isPlayingTrack01)
        {
            track02.clip = newClip;
            track02.Play();
            while(timeElapsed < timeToFade)
            {
                track02.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track01.volume = Mathf.Lerp(1, 0, timeElapsed / (timeToFade / 2));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            track01.Stop();
        }
        else
        {
            track01.clip = newClip;
            track01.Play();
            while (timeElapsed < timeToFade)
            {
                track01.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track02.volume = Mathf.Lerp(1, 0, timeElapsed / (timeToFade / 2));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            track02.Stop();
        }
    }
}
