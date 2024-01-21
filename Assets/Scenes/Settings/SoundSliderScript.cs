using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Required for Audio Mixer

public class SoundSliderScript : MonoBehaviour
{
    public AudioMixer audioMixer; // Assign your AudioMixer in the inspector
    private Slider soundSlider;

    void Start()
    {
        soundSlider = GetComponent<Slider>();
        float volume;
        audioMixer.GetFloat("SFXParam", out volume); // Get the current volume from the AudioMixer
        soundSlider.value = Mathf.Pow(10, volume / 20); // Convert the logarithmic value to linear slider value
        soundSlider.onValueChanged.AddListener(SetSoundVolume);
    }

    public void SetSoundVolume(float value)
    {
        // Convert the slider value to a logarithmic value that the mixer expects
        audioMixer.SetFloat("SFXParam", Mathf.Log10(value) * 20);
    }
}
