using System.Collections;
using UnityEngine;

public class Mine : MonoBehaviour
{
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _explosionTime = 2f;
    [SerializeField] private float _radius = 5f;
    [SerializeField] private float _power = 1000f;
    [SerializeField] private GameObject _particleObject;
    [SerializeField] private GameObject _mineBody;

    private bool _isDetonate = false;

    private void OnCollisionEnter(Collision collision)
    {
        if ((collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy")) && !_isDetonate)
        {
            Invoke(nameof(Explosion), _explosionTime);
            _isDetonate = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullets") && !_isDetonate)
        {
            Invoke(nameof(Explosion), 0f);
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
                }
                else
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
        Destroy(_mineBody);
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        Destroy(gameObject, 5f);
    }

    private void AddExpForce(Rigidbody rb)
    {
        rb.AddExplosionForce(_power, transform.position, _radius, 2.0F, ForceMode.Impulse);
        rb.AddRelativeTorque(Random.Range(1f, 10f), Random.Range(1f, 10f), Random.Range(1f, 10f), ForceMode.Impulse);
    }
}
