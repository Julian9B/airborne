using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private CameraController cameraTarget;
    [SerializeField] private UnitSystems unitSystems;

    [Header("Controls")]
    [SerializeField] private Button resetButton; 
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider uiSlider;
    [SerializeField] private Slider fovSlider;
    [SerializeField] private TMP_Dropdown unitDropdown;

    [Header("Default values")]
    [SerializeField] private float defaultMusicVolume = 0.5f;
    [SerializeField] private float defaultSFXVolume = 0.5f;
    [SerializeField] private float defaultUIVolume = 0.5f;
    [SerializeField] private float defaultFOV = 0.5f;
    [SerializeField] private int defaultSystemIndex = 0;

    private const string MUSIC_KEY = "MusicValue";
    private const string SFX_KEY = "SFXValue";
    private const string UI_KEY = "UIValue";
    private const string FOV_KEY = "FOVValue";
    private const string UNITS_KEY = "UnitSystemIndex";

    private void Start()
    {
        LoadUnitSystems();
        SetStartingSettings();

        resetButton.onClick.AddListener(ResetSettings);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        fovSlider.onValueChanged.AddListener(SetFOV);
        uiSlider.onValueChanged.AddListener(SetUIVolume);
        unitDropdown.onValueChanged.AddListener(SetUnitSystem);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(MUSIC_KEY, musicSlider.value);
        PlayerPrefs.SetFloat(SFX_KEY, sfxSlider.value);
        PlayerPrefs.SetFloat(UI_KEY, uiSlider.value);
        PlayerPrefs.SetFloat(FOV_KEY, fovSlider.value);
        PlayerPrefs.SetInt(UNITS_KEY, unitDropdown.value);
    }

    private void LoadUnitSystems()
    {
        unitDropdown.ClearOptions();

        foreach (UnitSystem system in unitSystems.systems)
        {
            unitDropdown.options.Add(new TMP_Dropdown.OptionData(system.systemName));
        }

        unitDropdown.RefreshShownValue();
    }

    private void SetStartingSettings()
    {
        // music volume
        float musicValue = PlayerPrefs.GetFloat(MUSIC_KEY, defaultMusicVolume);
        SetMusicVolume(musicValue);
        musicSlider.value = musicValue;

        // sound effects volume
        float sfxValue = PlayerPrefs.GetFloat(SFX_KEY, defaultSFXVolume);
        SetSFXVolume(sfxValue);
        sfxSlider.value = sfxValue;

        // ui volume
        float uiValue = PlayerPrefs.GetFloat(UI_KEY, defaultUIVolume);
        SetUIVolume(uiValue);
        uiSlider.value = uiValue;

        // fov
        float fov = PlayerPrefs.GetFloat(FOV_KEY, defaultFOV);
        SetFOV(fov);
        fovSlider.value = fov;

        // unit system
        int systemIndex = PlayerPrefs.GetInt(UNITS_KEY, defaultSystemIndex);
        SetUnitSystem(systemIndex);
        unitDropdown.value = systemIndex;
    }

    private void ResetSettings()
    {
        SetMusicVolume(defaultMusicVolume);
        musicSlider.value = defaultMusicVolume;

        SetSFXVolume(defaultSFXVolume);
        sfxSlider.value = defaultSFXVolume;

        SetUIVolume(defaultUIVolume);
        uiSlider.value = defaultUIVolume;

        SetFOV(defaultFOV);
        fovSlider.value = defaultFOV;

        SetUnitSystem(defaultSystemIndex);
        unitDropdown.value = defaultSystemIndex;
    }

    private void SetMusicVolume(float value)
    {
        audioManager.SetMusicVolume(value);
    }

    private void SetSFXVolume(float value)
    {
        audioManager.SetSFXVolume(value);
    }

    private void SetUIVolume(float value)
    {
        audioManager.SetUIVolume(value);
    }

    private void SetFOV(float value)
    {
        cameraTarget.SetFOV(value);
    }

    private void SetUnitSystem(int value)
    {
        UnitSystemManager.Instance.SetSystem(value);
    }
}
