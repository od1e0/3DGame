using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGun : MonoBehaviour, IWeapon
{
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform[] _bulletStartPosition;
    [SerializeField] private float _fireRate = 1.2f;
    [SerializeField] private GameObject _flashlight;

    public GameObject FlashLightPoint => _flashlight;
    private bool _isReload = true;

    public bool IsReload { get => _isReload; }

    public void Fire()
    {

        for (int i = 0; i < _bulletStartPosition.Length; i++)
        {
            Quaternion fractQuat = new Quaternion((this.transform.rotation.x + Random.Range(-0.15f, 0.15f)), 
                                                  (this.transform.rotation.y + Random.Range(-0.15f, 0.15f)), 
                                                  (this.transform.rotation.z + Random.Range(-0.15f, 0.15f)), 
                                                  (this.transform.rotation.w + Random.Range(-0.15f, 0.15f)));
            var fractions = Instantiate(_bulletPref, _bulletStartPosition[i].position, fractQuat);
        }
        _isReload = false;
        Invoke("Reload", _fireRate);
    }

    public void Fire(int modifer)
    {
        for (int i = 0; i < _bulletStartPosition.Length; i++)
        {
            Quaternion fractQuat = new Quaternion((this.transform.rotation.x + Random.Range(-0.15f, 0.15f)),
                                                  (this.transform.rotation.y + Random.Range(-0.15f, 0.15f)),
                                                  (this.transform.rotation.z + Random.Range(-0.15f, 0.15f)),
                                                  (this.transform.rotation.w + Random.Range(-0.15f, 0.15f)));
            var fractions = Instantiate(_bulletPref, _bulletStartPosition[i].position, fractQuat);
            fractions.GetComponent<Fractions>().modifer = modifer;
        }
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
