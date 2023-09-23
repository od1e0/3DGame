using UnityEngine.Rendering.PostProcessing;
using UnityEngine;
using System.Collections;

public class DarkLightShifter : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private GameObject _camera;

    private AutoExposure _autoExposure;
    private Bloom _bloom;
    private PostProcessEffectSettings[] _settings;
    private bool _isInDark;
    private bool _isOnLight;
    private float _bloomValue;

    private const float BrightFromLightToDark = 9f;
    private const float BrightInDark = 3f;
    private const float BrightFromDarkToLight = -5f;
    private const float SpeedUp = 7f;
    private const float SpeedDown = 2f;
    private const float BloomSpeedDown = 0.05f;
    private const int AdaptationTime = 2;

    private void Awake()
    {
        
        _autoExposure = ScriptableObject.CreateInstance<AutoExposure>();
        _bloom = ScriptableObject.CreateInstance<Bloom>();
        _autoExposure.enabled.Override(true);
        _bloom.enabled.Override(true);

        _settings = new PostProcessEffectSettings[] { _autoExposure, _bloom };
        _postProcessVolume = PostProcessManager.instance.QuickVolume(_camera.layer, 2, _settings);
    }

    private void Update()
    {
        if (_isInDark)
        {
            ShiftEntourage(BrightInDark, 335f);
        }

        if (_isOnLight)
        {
            ShiftEntourage(0f, 10f);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _isOnLight = false;

            SetEffectsSettings(BrightFromLightToDark, "Dark");

        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            _isInDark = false;

            SetEffectsSettings(BrightFromDarkToLight, "Light");

        }
    }

    private void ShiftEntourage(float bright, float targetBloom)
    {
        _autoExposure.speedDown.Override(SpeedDown);

        _autoExposure.minLuminance.Override(bright);
        _autoExposure.maxLuminance.Override(bright);

        _bloomValue = Mathf.Lerp(_bloomValue, targetBloom, BloomSpeedDown);
        _bloom.intensity.value = _bloomValue;
    }

    private void SetEffectsSettings(float bright, string environment)
    {
        _autoExposure.speedUp.Override(SpeedUp);
        _autoExposure.minLuminance.Override(bright);
        _autoExposure.maxLuminance.Override(bright);

        StartCoroutine(WaitForAdaptation(environment));
    }

    private IEnumerator WaitForAdaptation(string entourage)
    {
        int timeOut = 0;

        while(timeOut != AdaptationTime)
        {
            timeOut++;
            if (timeOut == AdaptationTime)
            {
                switch (entourage)
                {
                    case "Dark": 
                        _isInDark = true;
                        break;
                    case "Light":
                        _isOnLight = true;
                        break;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}   
