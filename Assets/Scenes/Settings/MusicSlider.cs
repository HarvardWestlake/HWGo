using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Required for Audio Mixer

public class MusicSliderScript : MonoBehaviour
{
    public AudioMixer audioMixer; // Assign your AudioMixer in the inspector
    private Slider musicSlider;
    private const string musicVolumeKey = "MusicVolume"; // A key for PlayerPrefs

    void Start()
    {
        musicSlider = GetComponent<Slider>();

        if (musicSlider == null)
    {
        Debug.LogError("MusicSlider component not found on the GameObject.");
        return; // Exit the method to prevent further errors
    }
        
        // Load the saved volume if it exists, use a default value otherwise
        float savedVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.75f); // Default value of 0.75 if not set
        musicSlider.value = savedVolume;
        
        // Apply the loaded value to the AudioMixer
        SetMusicVolume(savedVolume);

        // Update the AudioMixer volume when the slider's value changes
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    public void SetMusicVolume(float value)
    {
        // Convert the slider value to a logarithmic value that the mixer expects
        audioMixer.SetFloat("MusicParam", Mathf.Log10(value) * 20);

        // Save the current value to PlayerPrefs
        PlayerPrefs.SetFloat(musicVolumeKey, value);
        PlayerPrefs.Save();
    }
}
