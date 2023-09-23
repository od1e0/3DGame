using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightSettings : MonoBehaviour
{
    [SerializeField] private Color _skyColor;
    [SerializeField] private Color _equatorColor;
    [SerializeField] private Color _groundColor;
    [SerializeField] private Color _lightColor;

    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
        ChangeAmbientColor();
    }

    private void ChangeAmbientColor()
    {
        RenderSettings.ambientSkyColor = _skyColor;
        RenderSettings.ambientEquatorColor = _equatorColor;
        RenderSettings.ambientGroundColor = _groundColor;
        _light.color = _lightColor;
    }
}
