using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActions : MonoBehaviour, ITakingDamage
{

    //Params
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private int _maxSGAmmo = 50;
    [SerializeField] private int _maxMGAmmo = 500;
    [SerializeField] private GameObject _hpBar;
    [SerializeField] private GameObject _minesAndBombBar;
    [SerializeField] private GameObject _ammoBar;
    [SerializeField] private GameObject _head;
    [SerializeField] private GameObject _menuPanel;
    [SerializeField] private GameObject _endGamePanel;
    [SerializeField] private Image _hpBarImage;
    [SerializeField] private Text _hpText;
    [SerializeField] private Text _ammoText;
    [SerializeField] private Text _minesBombsText;
    [SerializeField] private AudioClip _walkAudio;
    [SerializeField] private AudioClip _runAudio;

    private Dictionary<Color, int> _keyContainer = new Dictionary<Color, int>
    {
        [new Color(1f, 0f, 0f, 1f)] = 0,
        [new Color(1f, 0.922f, 0.016f, 1f)] = 0,
        [new Color(0f, 0f, 1f, 1f)] = 0,
        [new Color(0f, 1f, 0f, 1f)] = 0
    };
    private int _hp;
    private int _sgAmmo = 50;
    private int _mgAmmo = 500;
    private int _curentWeaponAmmo;
    private int _curentWeaponMaxAmmo;
    private int _leverCount = 0;
    private int _secretBossDamageModifer = 1;
    private Animator animator;
    private AudioSource _playerAudioSource;
    private AudioSource _weaponAudioSource;
    private bool _isDead = false;
    private bool _isRun = false;
    private bool _isWalk = true;
    private Settings _settings;

    public bool IsDead { get => _isDead; }
    public AudioSource WeaponAudioSource { get => _weaponAudioSource; }
    public AudioSource PlayerAudioSource { get => _playerAudioSource; }
    public Dictionary<Color, int> KeyContainer { get => _keyContainer; set => _keyContainer = value; }
    public int LeverCount { get => _leverCount; set => _leverCount = value; }
    public int SecretBossDamageModifer { get => _secretBossDamageModifer; set => _secretBossDamageModifer = value; }
    public int MaxHP { get => _maxHP; }


    //Move Player
    [SerializeField] private float sensetivity;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMult;
    [SerializeField] private float _jumpForce = 300f;
    [SerializeField] private float _gravity = 9.18f;
    private bool _isGrounded;
    private Rigidbody _rb;
    private Vector3 _direction;
    private float mouseLookX = 0f;
    private float mouseLookY = 0f;
    private float xRotation = 0f;

    //Weapons and Shoting
    [SerializeField] private GameObject _mgPref;
    [SerializeField] private GameObject _sgPref;
    [SerializeField] private Transform _weaponPosition;
    [SerializeField] private Transform _weaponPositionAxie;
    [SerializeField] private GameObject _bombPref;
    [SerializeField] private Transform _bombStartPosition;
    [SerializeField] private GameObject _minePref;
    [SerializeField] private Transform _mineStartPosition;
    [SerializeField] private int _mineCount = 5;
    [SerializeField] private int _bombCount = 5;
    private GameObject _weaponPref;
    private GameObject weapon;
    private IWeapon w;
    private float _trowTime = 0f;
    private float _mineTime = 0f;
    private FlashLight FlashLight;
    private bool _isFlashLightOn;

    //GUI
    [SerializeField] private GameObject _hpBarObjectGUI;
    [SerializeField] private GameObject _tooltipObjectGUI;
    [SerializeField] private GameObject _colorSettingsGUI;
    [SerializeField] private GameObject _weaponChangerGUI;
    private HPBar _hpBarGUI;
    private LightColorChanger _colorSettings;
    private WeaponChanger _weaponChanger;
    private Tooltipes _tooltipes;

    private void Awake()
    {
        _hp = _maxHP;
        UpdateHPBar();
        _hpBarGUI = _hpBarObjectGUI.GetComponent<HPBar>();
        _hpBarGUI.UpdateHPBar(_hp);

        _colorSettings = _colorSettingsGUI.GetComponent<LightColorChanger>();
        _weaponChanger = _weaponChangerGUI.GetComponent<WeaponChanger>();
        _tooltipes = _tooltipObjectGUI.GetComponent<Tooltipes>();

        Cursor.lockState = CursorLockMode.Locked;
        PlayerPrefs.SetInt("_isShowTooltip", 0);


        _rb = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        _playerAudioSource = GetComponent<AudioSource>();

        _weaponPref = _mgPref;
        weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, transform.rotation);
        weapon.transform.parent = _weaponPositionAxie;
        w = weapon.GetComponent<MachineGun>();
        _weaponAudioSource = weapon.GetComponent<AudioSource>();
        FlashLight = w.FlashLightPoint.GetComponent<FlashLight>();
        _weaponAudioSource.Stop();
        _curentWeaponAmmo = _mgAmmo;
        _curentWeaponMaxAmmo = _maxMGAmmo;
        animator.SetBool("MGun", true);
        animator.SetBool("SGun", false);

        _settings = GameObject.FindObjectOfType<Settings>();
    }

    void Update()
    {
        //float f = 0f; // Генератор лагов =)
        //while (f < 100000f)
        //{
        //    f+=(0.5f * Time.deltaTime);
        //}
        //f = 0f;
        if (_isDead) return;
        
        PlayerLook();

        if (Input.GetKeyDown(KeyCode.Y)) 
        {
            Death();
        }
        if (Input.GetButton("Weapon1"))
        {
            SwitchWeapon(1);
        }
        else if (Input.GetButton("Weapon2"))
        {
            SwitchWeapon(2);
        }

        if (Input.GetAxis("Fire1") == 1f && w.IsReload && _curentWeaponAmmo > 0)
        {
            animator.SetBool("Fire", true);
            w.Fire(_secretBossDamageModifer);
            _curentWeaponAmmo--;
            if (animator.GetBool("MGun"))
            {
                WeaponSoundStart();
                _mgAmmo--;
            }
            else _sgAmmo--;

        }  else if (Input.GetButtonUp("Fire1") || _curentWeaponAmmo == 0)
        {
            if (animator.GetBool("MGun")) WeaponSoundStop();
            animator.SetBool("Fire", false);
        } 


        if (Input.GetAxis("Fire2") == 1f)
        {
            if (_trowTime == 0 && _bombCount > 0)
            {
                _bombCount--;
                TrowBomb();
            }

            _trowTime += Time.deltaTime;


            if (_trowTime > 1000F)
            {
                _trowTime = 0;
            }
        }
        else _trowTime = 0;


        if (Input.GetAxis("Fire3") == 1f)
        {
            if (_mineTime == 0 && _mineCount > 0)
            {
                _mineCount--;
                TrowMine();

            }

            _mineTime += Time.deltaTime;

            if (_mineTime > 1000F)
            {
                _mineTime = 0;
            }
        }
        else _mineTime = 0;

        if(Input.GetButton("Cancel"))
        {
            _hpBar.SetActive(false);
            _ammoBar.SetActive(false);
            _minesAndBombBar.SetActive(false);

            _tooltipes.IsPause = true;
            _hpBarGUI.IsPause = true;

            _menuPanel.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            AudioListener.volume = 0f;
        }

        if (Input.GetButtonDown("ColorSettings")) 
        { 
            _colorSettings.IsColorChangerButtonDown = true; 
        }


        if (Input.GetButtonDown("WeaponChanger"))
        {
            _weaponChanger.IsWeaponChangerButtonDown = true;
        }
        else if (Input.GetButtonUp("WeaponChanger")) 
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
            if (!(_settings is null))
            {
                AudioListener.volume = _settings.Volume * 0.01f;
            }
            else
            {
                AudioListener.volume = 0.5f;
            }
            _weaponChanger.IsWeaponChangerButtonDown = false; 
        }

        if (Input.GetButtonDown("FlashLight"))
        {
            if (_isFlashLightOn)
            {
                FlashLight.TurnOff();
                _isFlashLightOn = false;
            }
            else
            {
                FlashLight.TurnOn();
                _isFlashLightOn = true;
            }
        }


        _ammoText.text = "Ammo: " + _curentWeaponAmmo.ToString() + "/" + _curentWeaponMaxAmmo.ToString();
    }

    public void SwitchWeapon(int weaponType)
    {
        switch (weaponType)
        {
            case 1:
                animator.SetBool("MGun", true);
                animator.SetBool("SGun", false);
                w.DestroyWeapon();
                _weaponPref = _mgPref;
                weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, _head.transform.rotation);
                weapon.transform.parent = _weaponPositionAxie;
                w = weapon.GetComponent<MachineGun>();
                FlashLight = w.FlashLightPoint.GetComponent<FlashLight>();

                if (_isFlashLightOn) FlashLight.TurnOn();
                else FlashLight.TurnOff();

                _weaponAudioSource = weapon.GetComponent<AudioSource>();
                _weaponAudioSource.Stop();
                _curentWeaponAmmo = _mgAmmo;
                _curentWeaponMaxAmmo = _maxMGAmmo;
                break;

            case 2:
                animator.SetBool("MGun", false);
                animator.SetBool("SGun", true);
                w.DestroyWeapon();
                _weaponPref = _sgPref;
                weapon = Instantiate(_weaponPref, _weaponPositionAxie.position, _head.transform.rotation);
                weapon.transform.parent = _weaponPositionAxie;
                w = weapon.GetComponent<ShotGun>();
                FlashLight = w.FlashLightPoint.GetComponent<FlashLight>();

                if (_isFlashLightOn) FlashLight.TurnOn();
                else FlashLight.TurnOff();
 
                _weaponAudioSource = weapon.GetComponent<AudioSource>();
                _weaponAudioSource.Stop();
                _curentWeaponAmmo = _sgAmmo;
                _curentWeaponMaxAmmo = _maxSGAmmo;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (_direction != Vector3.zero)
        {
            animator.SetBool("Run", true);
        } else
        {
            animator.SetBool("Run", false);
            _playerAudioSource.Stop();
        }
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        Vector3 speed;
        if (Input.GetButton("Sprint"))
        {
            if (!_isRun) _playerAudioSource.Stop();

            if (!_playerAudioSource.isPlaying)
            {
                _playerAudioSource.clip = _runAudio;
                _playerAudioSource.loop = true;
                _playerAudioSource.Play();
                _isRun = true;
                _isWalk = false;
            }
            speed = _direction * (_speed * _speedMult);
        }
        else
        {
            if (!_isWalk) _playerAudioSource.Stop();
            if (!_playerAudioSource.isPlaying)
            {
                _playerAudioSource.clip = _walkAudio;
                _playerAudioSource.loop = true;
                _playerAudioSource.Play();
                _isRun = false;
                _isWalk = true;
            }
            speed = _direction * _speed;
        }

        JumpLogic();
        MovementLogic(speed);
    }

    
    private void PlayerLook()
    {
        mouseLookX = Input.GetAxis("Mouse X") * sensetivity * Time.deltaTime;
        mouseLookY = Input.GetAxis("Mouse Y") * sensetivity * Time.deltaTime;

        transform.Rotate(0, mouseLookX, 0);

        //xRotation += mouseLookY; // более правильная реализация, но ловит баг, в случае если продолжать двигать мышку, то xRotation продолжает изменяться

        //if (xRotation <= 45f && xRotation >= -45)
        //{

        //    _head.Rotate(mouseLookY, 0, 0);
        //    _weaponPosition.Rotate(mouseLookY, 0, 0);
        //}

        xRotation += mouseLookY; // Старая, не совсем верная реализация поворота головы и оружия, но без бага поворота
        xRotation = Mathf.Clamp(xRotation, -40f, 25f);
        _head.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        _weaponPosition.localRotation = _head.transform.localRotation;
        _bombStartPosition.localRotation = _head.transform.localRotation;
    }

    private void MovementLogic(Vector3 speed)
    {
        _rb.AddForce(transform.forward * speed.z, ForceMode.Impulse);
        _rb.AddForce(transform.right * speed.x, ForceMode.Impulse);
    }

    private void JumpLogic()
    {
        if (Input.GetAxis("Jump") > 0)
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpForce * 100);
            }
        }

        if (!_isGrounded)
        {
            _rb.AddForce(Vector3.down * _gravity * 300);
        }
    }

    void OnCollisionEnter(Collision collision) // Устанавливаем флаг Ground при приземлении
    {
        IsGroundedUpate(collision, true);
    }

    void OnCollisionExit(Collision collision) // Снимаем флаг Ground при прыжке
    {
        IsGroundedUpate(collision, false);
    }

    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("Ground"))
        {
            _isGrounded = value;
        }
    }

    private void TrowBomb()
    {
        var bomb = Instantiate(_bombPref, _bombStartPosition.position, transform.rotation);
        bomb.GetComponent<Rigidbody>().AddForce(_bombStartPosition.forward * 20, ForceMode.Impulse);
        var b = bomb.GetComponent<Bomb>();
        b.Init();
        _minesBombsText.text = "Mines: " + _mineCount.ToString() + " Bombs: " + _bombCount.ToString();
    }

    private void TrowMine()
    {
        var mine = Instantiate(_minePref, _mineStartPosition.position, transform.rotation);
        _minesBombsText.text = "Mines: " + _mineCount.ToString() + " Bombs: " + _bombCount.ToString();
    }


    public void TakingDamage(int damage, Transform sourceDamage)
    {
        if (_isDead) return;
        _head.GetComponent<AudioSource>().Play();
        _head.GetComponent<ParticleSystem>().Play();
        _hp -= damage;
        UpdateHPBar();
        _hpBarGUI.UpdateHPBar(_hp);
        if (_hp <= 0)
        {
            Death();
        }
    }

    public void TakingBombDamage(int damage)
    {
        if (_isDead) return;
        _hp -= damage;
        UpdateHPBar();
        _hpBarGUI.UpdateHPBar(_hp);
        if (_hp <= 0)
        {
            Invoke("Death", 1f);
        }
    }

    private void UpdateHPBar()
    {
        _hpText.text = _hp.ToString() + "/" + _maxHP;
        float fill = (((float)_hp * 100) / (float)_maxHP) / 100;
        _hpBarImage.fillAmount = fill;
    }


    private void Death()
    {
        animator.SetTrigger("Death");
        _isDead = true;
        AudioListener.volume = 0;
        Cursor.lockState = CursorLockMode.None;
        _endGamePanel.SetActive(true);
    }

    public void GetHeal(int healCount)
    {
        Debug.Log("Was " + _hp);
        _hp += healCount;
        if (_hp > _maxHP) _hp = _maxHP;
        UpdateHPBar();
        Debug.Log("Became " + _hp);
    }

    public void GetAmmo(int sgAmmoCount, int mgAmmoCount)
    {
        _sgAmmo += sgAmmoCount;       
        _mgAmmo += mgAmmoCount;

        if (_mgAmmo > _maxMGAmmo) _mgAmmo = _maxMGAmmo;
        if (_sgAmmo > _maxSGAmmo) _sgAmmo = _maxSGAmmo;

        if (animator.GetBool("MGun"))
        {
            _curentWeaponAmmo = _mgAmmo;
        }
        else
        {
            _curentWeaponAmmo = _sgAmmo;
        }

    }

    public void AddLeverCount()
    {
        LeverCount += 1;
        Debug.Log("Add Lever. Now " + LeverCount);
    }

    public void AddKey(Color color)
    {

        if (_keyContainer.TryGetValue(color, out int value))
        {
            _keyContainer[color] = 1;
        }

        foreach (var item in _keyContainer)
        {
            Debug.Log(item.Key + ": " + item.Value);
        }
    }

    private void WeaponSoundStart()
    {

        if (!_weaponAudioSource.isPlaying)
            _weaponAudioSource.Play();
    }

    private void WeaponSoundStop()
    {
        _weaponAudioSource.Stop();
    }
}
