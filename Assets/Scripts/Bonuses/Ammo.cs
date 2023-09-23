using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    [SerializeField] private int _sgAmmoCount = 10;
    [SerializeField] private int _mgAmmoCount = 100;

    private void Update()
    {
        //transform.Rotate(0, 1, 0);
        //transform.position = new Vector3(transform.position.x, 1f + Mathf.Sin(Time.fixedTime*3f) * 0.2f, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>().GetAmmo(_sgAmmoCount, _mgAmmoCount);
            Debug.Log("GetAmmo!!");
            Destroy(gameObject);
        }
    }
}
