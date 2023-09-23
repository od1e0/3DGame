using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachineGun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private GameObject _flashlight;

    public GameObject FlashLightPoint => _flashlight;
    private bool _isReload = true;

    public bool IsReload { get => _isReload; }

    public void Fire()
    {
        var bullet = Instantiate(_bulletPref, _bulletStartPosition.position, transform.rotation);
        _isReload = false;
        Invoke("Reload", _fireRate);
    }

    public void Fire(int modifer)
    {
        var bullet = Instantiate(_bulletPref, _bulletStartPosition.position, transform.rotation);
        bullet.GetComponent<Bullet>().modifer = modifer;
        _isReload = false;
        Invoke("Reload", _fireRate);
    }

    private void Reload()
    {
        _isReload = true;
    }

    public void DestroyWeapon()
    {
        Destroy(gameObject);
    }


}
