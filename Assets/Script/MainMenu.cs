using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Audio;
using System.IO;

public class MainMenu : MonoBehaviour
{
    public static bool loadSavedData;

    [Header("UI References")]
    [SerializeField] private Button clearDataButton;
    [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private Dropdown resolutionDropdown;
    public Slider volumeSlider;
    [SerializeField] private GameObject pausepanel;

    [Header("Audio")]
    [SerializeField] SaveSystem saveSystem;
    [SerializeField] Button saveButton;
    [SerializeField] Toggle FullCreenToogle;
    public AudioMixer audioMixer;
    public bool autoSaveEnabled;
    [SerializeField] Toggle autoSaveToggle;

    private string audioFilePath => Path.Combine(Application.persistentDataPath, "audioSettings.json");

    [System.Serializable]
    public class AudioData
    {
        public float volume;
    }

    private void Start()
{
    float currentVolume = LoadAudioSettings();
    volumeSlider.value = currentVolume;
    ApplyVolume(currentVolume);

    FullCreenToogle.isOn = Screen.fullScreen;

    clearDataButton.interactable = File.Exists(Application.persistentDataPath + "/savedData.json");

    optionsPanel.SetActive(false);

    SetupResolutionDropdown();
    SetupQualityDropdown();

    // 👉 Charger l’état du toggle AutoSave
    LoadAutoSaveState();
}


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausepanel != null)
            {
                bool newState = !pausepanel.activeSelf;
                pausepanel.SetActive(newState);

                if (!newState)
                    optionsPanel.SetActive(false);

                Time.timeScale = newState ? 0f : 1f;
            }
            else
            {
                Debug.LogWarning("pausepanel n'est pas assigné dans l'inspector !");
            }
        }

        if (saveButton != null)
        {
            saveButton.interactable = saveSystem.HasSaveChanged();
            clearDataButton.interactable = !saveSystem.HasSaveChanged();
        }
        if (autoSaveEnabled)
        {
            if(saveSystem!= null)
            {
            saveSystem.SaveData();
            }
            if(saveButton!= null)
            {
            saveButton.interactable = false;
            }
        }
    }

    private float LoadAudioSettings()
    {
        if (!File.Exists(audioFilePath))
        {
            audioMixer.GetFloat("Volume", out float defaultVol);
            SaveAudioSettings(defaultVol);
            return defaultVol;
        }
        string json = File.ReadAllText(audioFilePath);
        AudioData data = JsonUtility.FromJson<AudioData>(json);
        return data.volume;
    }

    public void SaveAudioSettings(float volume)
    {
        AudioData data = new AudioData { volume = volume };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(audioFilePath, json);
    }

    public void ApplyVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetVolume(float volume)
    {
        ApplyVolume(volume);
        SaveAudioSettings(volume);
    }

    private void SetupQualityDropdown()
    {
        string[] qualities = QualitySettings.names;
        qualityDropdown.ClearOptions();
        List<string> qualityOptions = new List<string>();
        int currentQualityIndex = 0;
        for (int i = 0; i < qualities.Length; i++)
        {
            qualityOptions.Add(qualities[i]);
            if (i == QualitySettings.GetQualityLevel())
            {
                currentQualityIndex = i;
            }
        }
        qualityDropdown.AddOptions(qualityOptions);
        qualityDropdown.value = currentQualityIndex;
        qualityDropdown.RefreshShownValue();
    }

    public void SaveData()
    {
        saveSystem.SaveData();
    }

    private void SetupResolutionDropdown()
    {
        Resolution[] resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " (" + resolutions[i].refreshRate + "Hz)";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height &&
                resolutions[i].refreshRate == Screen.currentResolution.refreshRate)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void LoadGameButton()
    {
        loadSavedData = true;
        SceneManager.LoadScene("Scene");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = Screen.resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen, resolution.refreshRate);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

public void ClearDataButton()
{
    string saveFile = Application.persistentDataPath + "/savedData.json";
    if (File.Exists(saveFile))
        File.Delete(saveFile);

    string audioFile = Path.Combine(Application.persistentDataPath, "audioSettings.json");
    if (File.Exists(audioFile))
        File.Delete(audioFile);

    ApplyVolume(0f);

    clearDataButton.interactable = false;
    SceneManager.LoadScene("MainMenu");
    Time.timeScale = 1;
    volumeSlider.value = 0f;
    PlayerPrefs.DeleteAll();
}



    public void EnableOptionsPanel()
    {
        optionsPanel.SetActive(true);
    }

    public void DisableOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }
    public void OnAutoSaveToggleChanged(bool value)
{
    autoSaveEnabled = value;

    PlayerPrefs.SetInt("AutoSave", value ? 1 : 0);
    PlayerPrefs.Save();
}
    private void LoadAutoSaveState()
{
    
    if (!PlayerPrefs.HasKey("AutoSave"))
    {
        // Premier lancement → décoché
        if(autoSaveToggle)
        {
        autoSaveToggle.isOn = false;
        }
        autoSaveEnabled = false;
        PlayerPrefs.SetInt("AutoSave", 0);
        PlayerPrefs.Save();
    }
    else
    {
        // Chargement de la valeur sauvegardée
        int savedValue = PlayerPrefs.GetInt("AutoSave");
        autoSaveEnabled = savedValue == 1;
        if(autoSaveToggle)
        {
        autoSaveToggle.isOn = autoSaveEnabled;
        }
    }
}


}
