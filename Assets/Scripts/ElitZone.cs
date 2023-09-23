using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElitZone : MonoBehaviour
{
    [SerializeField] private GameObject _elitEnemy;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && _elitEnemy != null)
            _elitEnemy.GetComponent<EliteEnemy>()._isCanTakeDamage = true;
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && _elitEnemy != null)
            _elitEnemy.GetComponent<EliteEnemy>()._isCanTakeDamage = false;
    }
}
