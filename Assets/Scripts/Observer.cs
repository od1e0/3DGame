using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    [SerializeField] private GameObject _body;
    bool m_IsPlayerInRange = false;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
    private GameObject _player;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_IsPlayerInRange = true;
            _player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_IsPlayerInRange = false;
        }
    }

    void Update()
    {
        var enemy = _body.GetComponent<RegEnemy>();
        if (m_IsPlayerInRange && !_player.GetComponent<PlayerActions>().IsDead)
        {
            
            enemy.StopPatrol();
            enemy.StartAttack(_player);

        } else
        {
            StartCoroutine(GoBackDelay(enemy));
        }
    }

    private IEnumerator GoBackDelay(RegEnemy enemy)
    {
        int i = 0;
        while (i <= 20)
        {
            if (i == 20) enemy.EndAttack(enemy.SpawnPosition);
            yield return _waitForFixedUpdate;
            i++;
        }
    }
}
