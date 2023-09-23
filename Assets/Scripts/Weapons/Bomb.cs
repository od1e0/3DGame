using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _power = 5000f;
    [SerializeField] private GameObject _particleObject;
    [SerializeField] private GameObject _bombBody;

    private bool _isDetonate = false;

    public void Init()
    {
        Debug.Log("BombIsOut");
        Invoke("Explosion", 3f);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && !_isDetonate)
        {
            Explosion();
            _isDetonate = true;
        }
    }

    private void Explosion()
    {
        var colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (var hit in colliders)
        {
            if (hit.gameObject.CompareTag("Player") || hit.gameObject.CompareTag("Enemy") || hit.gameObject.CompareTag("Movable"))
            {
                Debug.Log(hit.gameObject.name);
                Rigidbody rb = hit.GetComponent<Rigidbody>();
                IEnemy enemy = hit.GetComponent<IEnemy>();

                if (hit.gameObject.CompareTag("Movable"))
                {
                    AddExpForce(rb);
                } else
                {
                    if (rb.isKinematic == true && hit.gameObject.CompareTag("Enemy"))
                    {
                        enemy.IsBombed();
                        AddExpForce(rb);
                    }
                    else AddExpForce(rb);
                    hit.GetComponent<ITakingDamage>().TakingBombDamage(_damage);
                }

            }
        }
        _particleObject.SetActive(true);
        GetComponent<AudioSource>().Play();
        Destroy(_bombBody);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<SphereCollider>().enabled = false;
        Destroy(gameObject, 5f);
    }

    private void AddExpForce(Rigidbody rb)
    {
        rb.AddExplosionForce(_power, transform.position, _radius, 2.0F, ForceMode.Impulse);
        rb.AddRelativeTorque(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f), ForceMode.Impulse);
    }
}
