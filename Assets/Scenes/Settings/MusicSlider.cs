using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class MusicSliderScript : MonoBehaviour
{
    public AudioMixer AudioMixer; // Assign your AudioMixer in the inspector
    private Slider musicSlider;

    void Start()
    {
        musicSlider = GetComponent<Slider>();
        float volume;
        AudioMixer.GetFloat("MusicParam", out volume); // Get the current volume from the AudioMixer
        musicSlider.value = Mathf.Pow(10, volume / 20); // Convert the logarithmic value to linear slider value
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float value)
    {
        // Convert the slider value to a logarithmic value that the mixer expects
        AudioMixer.SetFloat("MusicParam", Mathf.Log10(value) * 20);
    }
}
