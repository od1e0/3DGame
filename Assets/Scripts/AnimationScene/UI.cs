using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class UI : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private PostProcessProfile _clearProfile;
    [SerializeField] private Button _clearButton;

    [SerializeField] private Button _lowHPButton;
    [SerializeField] private PostProcessProfile _lowHPProfile;

    [SerializeField] private Button _fastMoveButton;
    [SerializeField] private PostProcessProfile _fastMoveProfile;

    private enum Effects
    {
        Clear,
        LowHP,
        Fast
    }

    private void Awake()
    {
        _clearButton.onClick.AddListener(() => OnProfile(Effects.Clear));
        _lowHPButton.onClick.AddListener(() => OnProfile(Effects.LowHP));
        _fastMoveButton.onClick.AddListener(() => OnProfile(Effects.Fast));
    }

    private void OnDestroy()
    {
        _clearButton.onClick.RemoveAllListeners();
        _lowHPButton.onClick.RemoveAllListeners();
    }

    private void OnProfile(Effects effect)
    {
        _postProcessVolume.profile = effect switch
        {
            Effects.Clear => _clearProfile,
            Effects.LowHP => _lowHPProfile,
            Effects.Fast => _fastMoveProfile
        };
    }
}
