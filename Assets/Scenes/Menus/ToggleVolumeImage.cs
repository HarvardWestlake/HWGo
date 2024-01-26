using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleVolumeImage : MonoBehaviour
{
    public Image ImageDisplayGameObject;
    public Sprite ImageToActive;

    public Sprite oldPic;

    int i = 0;
    public void ShowImageandMute()
    {
        if(i%2 != 0)
        {
            AudioListener.volume = 0;
            ImageDisplayGameObject.sprite = ImageToActive;
        }
    
    else
    {
        AudioListener.volume = 1;
        ImageDisplayGameObject.sprite = oldPic;
    }

        i++;
    }
    
}
