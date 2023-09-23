using UnityEngine;
using UnityEngine.UI;

public class SettingsInGame : MonoBehaviour
{
    [SerializeField] Toggle _tglSoundsMute;
    [SerializeField] Slider _slrSoundsVolume;
    [SerializeField] Text _txtVolumePercent;
    [SerializeField] Button _btnSaveSettings;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _pausePanel;
    [SerializeField] Dropdown _ddQuality;

    private Settings _settings;
    private int _qualityLevel;
    private bool _isMuted;
    private float _volume;

    private void Awake()
    {
        _settings = GameObject.FindObjectOfType<Settings>(); //А как сделать без поиска?
        _qualityLevel = _settings.Quality;
        _isMuted = _settings.IsMuted;
        _volume = _settings.Volume;
        _txtVolumePercent.text = _settings.Volume.ToString();
        AudioListener.volume = _volume * 0.01f;

        _ddQuality.value = _qualityLevel;
        _tglSoundsMute.isOn = _isMuted;
        _slrSoundsVolume.value = _volume;

        _btnSaveSettings.onClick.AddListener(SaveSettings);
        _tglSoundsMute.onValueChanged.AddListener(SoundsMute);
        _slrSoundsVolume.onValueChanged.AddListener(Volume);
    }

    private void SaveSettings()
    {
        QualitySettings.SetQualityLevel(_ddQuality.value, true);
        _qualityLevel = _ddQuality.value;
        _volume = _slrSoundsVolume.value;
        _settings.SetSettings(_volume, _isMuted, _qualityLevel);
        AudioListener.volume = _volume * 0.01f;

        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(true);
    }
    private void SoundsMute(bool value)
    {
        _isMuted = value;
        if (_isMuted)
        {
            _slrSoundsVolume.value = 0;
            _slrSoundsVolume.interactable = false;
        }
        else
        {
            _slrSoundsVolume.interactable = true;
        }
    }

    private void Volume(float value)
    {
        _txtVolumePercent.text = value.ToString();
    }
}
