using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChanger : MonoBehaviour
{
    [SerializeField] GameObject _player;

    private Color myColor;
    private bool _isWeaponChangerButtonDown = false;
    private Settings _settings;
    private PlayerActions _playerActions => _player.GetComponent<PlayerActions>();

    public bool IsWeaponChangerButtonDown { set => _isWeaponChangerButtonDown = value; }

    private void Awake()
    {
        _settings = GameObject.FindObjectOfType<Settings>();
    }

    private void OnGUI()
    {
        if (_isWeaponChangerButtonDown)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            AudioListener.volume = 0f;
            GUI.Box(new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 50, 200, 100), "Weapons");


            if (GUI.Button(new Rect((Screen.width / 2) + 10, (Screen.height / 2), 70, 30), "ShotGun"))
            {
                _playerActions.SwitchWeapon(2);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                _isWeaponChangerButtonDown = false;
                if (!(_settings is null))
                {
                    AudioListener.volume = _settings.Volume * 0.01f;
                }
                else
                {
                    AudioListener.volume = 0.5f;
                }
            }

            if (GUI.Button(new Rect((Screen.width / 2) - 80, (Screen.height / 2), 70, 30), "MGun"))
            {
                _playerActions.SwitchWeapon(1);
                Cursor.lockState = CursorLockMode.Locked;
                Time.timeScale = 1f;
                _isWeaponChangerButtonDown = false;
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
}
