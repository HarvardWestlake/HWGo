using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Required for Audio Mixer
using TMPro;

public class MusicSliderScript : MonoBehaviour
{
    public AudioMixer audioMixer; // Assign your AudioMixer in the inspector
    public Slider musicSlider;
    private const string musicVolumeKey = "MusicVolume"; // A key for PlayerPrefs
    public TextMeshProUGUI musicVolumeText;

    void Start()
    {
        // musicSlider = GetComponent<Slider>();

        // Load the saved volume if it exists, use a default value otherwise
        float savedVolume = PlayerPrefs.GetFloat(musicVolumeKey, 0.75f); // Default value of 0.75 if not set
        musicSlider.value = savedVolume;

        // Apply the loaded value to the AudioMixer
        SetMusicVolume(savedVolume);

        // Update the AudioMixer volume when the slider's value changes
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        musicVolumeText.text = (savedVolume * 100).ToString("0") + " <#636363>/ 100";
    }

    public void SetMusicVolume(float value)
    {
        // Convert the slider value to a logarithmic value that the mixer expects
        audioMixer.SetFloat("MusicParam", Mathf.Log10(value) * 20);

        // Save the current value to PlayerPrefs
        PlayerPrefs.SetFloat(musicVolumeKey, value);
        PlayerPrefs.Save();

        musicVolumeText.text = (value * 100).ToString("0") + " <#636363>/ 100";
    }
}
