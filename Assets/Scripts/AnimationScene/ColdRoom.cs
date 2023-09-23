using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ColdRoom : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private PostProcessProfile _clearProfile;
    [SerializeField] private PostProcessProfile _coldProfile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _postProcessVolume.profile = _coldProfile;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _postProcessVolume.profile = _clearProfile;
        }
    }
}
