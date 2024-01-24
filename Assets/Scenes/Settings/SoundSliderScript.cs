using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio; // Required for Audio Mixer
using TMPro;

public class SouldSliderScript : MonoBehaviour
{
    public AudioMixer audioMixer; // Assign your AudioMixer in the inspector
    private Slider soundSlider;
    private const string soundVolumeKey = "SoundVolume"; // A key for PlayerPrefs
    public TextMeshProUGUI soundVolumeText;

    void Start()
    {
        soundSlider = GetComponent<Slider>();

        // Load the saved volume if it exists, use a default value otherwise
        float savedVolume = PlayerPrefs.GetFloat(soundVolumeKey, 0.75f); // Default value of 0.75 if not set
        soundSlider.value = savedVolume;

        // Apply the loaded value to the AudioMixer
        setSoundVolume(savedVolume);

        // Update the AudioMixer volume when the slider's value changes
        soundSlider.onValueChanged.AddListener(setSoundVolume);

        soundVolumeText.text = (savedVolume * 100).ToString("0") + " <#636363>/ 100";
    }

    public void setSoundVolume(float value)
    {
        // Convert the slider value to a logarithmic value that the mixer expects
        audioMixer.SetFloat("SFXParam", Mathf.Log10(value) * 20);

        // Save the current value to PlayerPrefs
        PlayerPrefs.SetFloat(soundVolumeKey, value);
        PlayerPrefs.Save();

        soundVolumeText.text = (value * 100).ToString("0") + " <#636363>/ 100";
    }
}
