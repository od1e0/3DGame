using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Focusing : MonoBehaviour
{
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private Transform _body;
    [SerializeField] private LayerMask _rayLayer;

    private DepthOfField _depthOfField;
    private const float CameraDistance = 2f;

    private void Awake()
    {
        _depthOfField = ScriptableObject.CreateInstance<DepthOfField>();

        _postProcessVolume = PostProcessManager.instance.QuickVolume(gameObject.layer, 2, _depthOfField);
    }

    private void Update()
    {
        if (Physics.Raycast(_body.position, _body.transform.TransformDirection(Vector3.forward), out var hit, 5f, _rayLayer))

        {
            _depthOfField.enabled.Override(true);
            _depthOfField.focusDistance.value = hit.distance + CameraDistance;
            Debug.Log(hit.transform.tag);
        } else
        {
            _depthOfField.enabled.Override(false);
        }
    }
}
