using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    [SerializeField] private Texture _hpBarTexture;
    [SerializeField] private Texture _hpBarBack;
    [SerializeField] private GameObject _player;

    private float _barWidth;
    private float _textureRedWidth;
    private float _textureBlackWidth;
    private int _hp;
    private int _maxHp;

    private bool _isPause;
    public bool IsPause { set => _isPause = value; }

    private void Awake()
    {
        _barWidth = Screen.width / 5;
        _textureRedWidth = _barWidth;
        _textureBlackWidth = _barWidth;
        _maxHp = _player.GetComponent<PlayerActions>().MaxHP;
    }

    private void OnGUI()
    {
        if (!_isPause)
        {
            GUI.Box(new Rect(5, 10, _barWidth, 40), _hp + "/" + _maxHp);
            GUI.DrawTexture(new Rect(5, 30, _textureBlackWidth, 15), _hpBarBack, ScaleMode.ScaleAndCrop, true, 10.0f);

            if (_hpBarTexture != null && _hpBarTexture.width > 0)
            {
                GUI.DrawTexture(new Rect(5, 30, _textureRedWidth, 15), _hpBarTexture, ScaleMode.ScaleAndCrop, true, 10.0f);
            }
        }
    }

    public void UpdateHPBar(int hp)
    {
        _hp = hp;
        _textureRedWidth = _barWidth * ((float)_hp / (float)_maxHp);
    }
}
