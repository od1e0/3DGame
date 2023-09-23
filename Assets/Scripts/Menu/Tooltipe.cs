using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tooltipe : MonoBehaviour
{
    [SerializeField] Button _btnOk;

    Settings _settings;

    private void Awake()
    {
        _btnOk.onClick.AddListener(Close);
        _settings = GameObject.FindObjectOfType<Settings>();
    }

    private void Close()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1f;
        if (!(_settings is null))
        {
            AudioListener.volume = _settings.Volume * 0.01f;
        } else
        {
            AudioListener.volume = 0.5f;
        }    
    }
}
