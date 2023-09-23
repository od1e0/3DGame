using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlashLight : MonoBehaviour
{
   
    private Light _light;
    private void Awake()
    {
        _light = GetComponent<Light>();
    }
    public void TurnOff()
    {
        _light.enabled = false;
    }

    public void TurnOn()
    {
        _light.enabled = true;
    }
}
