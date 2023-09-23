using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossArea : MonoBehaviour
{
    [SerializeField] private GameObject _boss;

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && _boss != null)
            if (!collision.gameObject.GetComponent<PlayerActions>().IsDead)
            {
                _boss.GetComponent<Boss>().Attack(collision.gameObject);
            } else
            {
                _boss.GetComponent<Boss>().EndAttack();
            }
            
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && _boss != null)
            _boss.GetComponent<Boss>().EndAttack();
    }
}
