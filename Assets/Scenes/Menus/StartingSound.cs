using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingSound : MonoBehaviour
{
   public  AudioSource audio;

   public void playButton()
   {
       audio.Play();
   }
}
