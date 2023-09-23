using UnityEngine;

public class Settings : MonoBehaviour
{
    private bool _isMuted;
    private float _volume;
    private int _qualityLevel;

    public bool IsMuted { get => _isMuted; }
    public float Volume { get => _volume; }

    public int Quality { get => _qualityLevel; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SetSettings(float soundVolume, bool mute, int qualityLevel)
    {
        _isMuted = mute;
        _volume = soundVolume;
        _qualityLevel = qualityLevel;
    }
}
