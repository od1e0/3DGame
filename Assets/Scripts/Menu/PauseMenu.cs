using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] Button _btnResume;
    [SerializeField] Button _btnExit;
    [SerializeField] GameObject _hpBar;
    [SerializeField] private GameObject _minesAndBombBar;
    [SerializeField] private GameObject _ammoBar;

    //GUI
    [SerializeField] private GameObject _hpBarObjectGUI;
    [SerializeField] private GameObject _tooltipObjectGUI;
    private Tooltipes _tooltipes => _tooltipObjectGUI.GetComponent<Tooltipes>();
    private HPBar _hpBarGUI => _hpBarObjectGUI.GetComponent<HPBar>();

    private Settings _settings;

    private void Awake()
    {
        
        _btnResume.onClick.AddListener(ResumeGame);
        _btnExit.onClick.AddListener(ExitGame);
        _settings = GameObject.FindObjectOfType<Settings>();

    }

    private void ExitGame()
    {
        Application.Quit();
    }

    private void ResumeGame()
    {
        gameObject.SetActive(false);
        _hpBar.SetActive(true);
        _ammoBar.SetActive(true);
        _hpBarGUI.IsPause = false;
        _tooltipes.IsPause = false;
        _minesAndBombBar.SetActive(true);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
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
