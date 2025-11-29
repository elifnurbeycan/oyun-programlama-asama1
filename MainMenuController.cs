using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [Header("UI References")]
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
        float sfx   = PlayerPrefs.GetFloat("SFXVolume", 0.8f);

        musicSlider.value = music;
        sfxSlider.value   = sfx;

        // Şimdilik basit çözüm: master volume = music slider
        AudioListener.volume = music;
    }

    public void OnNewGameClicked()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OnMusicSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        AudioListener.volume = value;
    }

    public void OnSFXSliderChanged(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        // İleride SFX AudioSource volume'lerini buradan ayarlayacağız
    }
}
