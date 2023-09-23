using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractions : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage = 5;

    public int modifer = 1;
    public void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        {
            if (other.CompareTag("Enemy"))
            {
                other.GetComponent<ITakingDamage>().TakingDamage(_damage * modifer, gameObject.transform);
            }
            else if (other.CompareTag("EnemyExtra"))
            {
                other.GetComponentInParent<ITakingDamage>().TakingDamage(_damage * 2 * modifer, gameObject.transform);
            }

            if (!other.CompareTag("Bullets") && !other.CompareTag("Traps") && !other.CompareTag("Weapon") && !other.CompareTag("Vision"))
            {
                Destroy(gameObject);
            }
        }
    }
}
