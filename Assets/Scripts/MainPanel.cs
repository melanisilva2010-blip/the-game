using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{
    [Header("Option")]
    public Slider volumen;
    public Slider FXvolumen;
    public Toggle mute;
    public AudioMixer mixer;
    public AudioSource fxSource;
    public AudioClip clickSound;
    private float lastVolume;
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject optionPanel;
    public GameObject playPanel;

    private void Awake()
    {
        volumen.onValueChanged.AddListener(ChangeVolumenMaster);
        FXvolumen.onValueChanged.AddListener(ChangeVolumenFX);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetMute()
    {

        if (mute.isOn)
        {
            mixer.GetFloat("VolMaster", out float lastVolume);
            mixer.SetFloat("VolMaster", -80);
        }
        else
            mixer.SetFloat("VolMaster", lastVolume);

    }
    public void OpenPanel(GameObject panel)
    {
        mainPanel.SetActive(false);
        optionPanel.SetActive(false);
        //playPanel.SetActive(false);
        PlaySoundButton();
        panel.SetActive(true);
    }
    public void ChangeVolumenMaster(float v)
    {
        mixer.SetFloat("VolMaster", v);
    }
    public void ChangeVolumenFX(float v)
    {
        mixer.SetFloat("VolFX", v);
    }
    public void CambiarNivel()
    {
        SceneManager.LoadScene("introduction");
    }
    public void PlaySoundButton()
    {
        fxSource.PlayOneShot(clickSound);
    }
}

