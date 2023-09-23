using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
    [SerializeField] GameObject _directionalLight;

    private Color myColor;
    private bool _isColorChangerButtonDown = false;
    private Settings _settings;

    public bool IsColorChangerButtonDown { set => _isColorChangerButtonDown = value; }

    private void Awake()
    {
        myColor = _directionalLight.GetComponent<Light>().color;
        _settings = GameObject.FindObjectOfType<Settings>();
    }

    private void OnGUI()
    {
        if (_isColorChangerButtonDown)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            AudioListener.volume = 0f;
            GUI.Box(new Rect((Screen.width/2) - 200, (Screen.height/2) - 150, 400, 300), "Color Settings");
            myColor = RGBSlider(new Rect((Screen.width / 2) - 150, (Screen.height / 2) - 100, 200, 20), myColor);

            if (GUI.Button(new Rect((Screen.width / 2) + 130, (Screen.height / 2) + 110, 60, 30), "Chancel"))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                _isColorChangerButtonDown = false;
                if (!(_settings is null))
                {
                    AudioListener.volume = _settings.Volume * 0.01f;
                }
                else
                {
                    AudioListener.volume = 0.5f;
                }
            }

            if (GUI.Button(new Rect((Screen.width / 2) + 60, (Screen.height / 2) + 110, 60, 30), "Save"))
            {
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                _directionalLight.GetComponent<Light>().color = myColor;
                _isColorChangerButtonDown = false;
                if (!(_settings is null))
                {
                    AudioListener.volume = _settings.Volume * 0.01f;
                }
                else
                {
                    AudioListener.volume = 0.5f;
                }
            }
        }

    }
    private Color RGBSlider(Rect screenRect, Color rgb)
    {
        rgb.r = LabelSlider(screenRect, rgb.r, 0.0f, 1.0f, "Red", rgb.r);
        screenRect.y += 40;
        rgb.g = LabelSlider(screenRect, rgb.g, 0.0f, 1.0f, "Green", rgb.g);
        screenRect.y += 40;
        rgb.b = LabelSlider(screenRect, rgb.b, 0.0f, 1.0f, "Blue", rgb.b);
        screenRect.y += 40;
        rgb.a = LabelSlider(screenRect, rgb.a, 0.0f, 1.0f, "Alpha", rgb.a);
        return rgb;
    }
    private float LabelSlider(Rect screenRect, float sliderValue, float sliderMinValue, float sliderMaxValue, string labelText, float value)
    {
        GUI.Label(screenRect, labelText);
        screenRect.x += screenRect.width / 2;
        sliderValue = GUI.HorizontalSlider(screenRect, sliderValue, sliderMinValue, sliderMaxValue);
        screenRect.x += screenRect.width / 2 - 10;
        screenRect.y += screenRect.height - 5;
        GUI.Label(screenRect, value.ToString("f2")) ;
        return sliderValue;
    }
}
