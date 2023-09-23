using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverElite : MonoBehaviour
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
        var enemy = _body.GetComponent<EliteEnemy>();
        if (m_IsPlayerInRange && !_player.GetComponent<PlayerActions>().IsDead)
        {
            enemy.StartAttack();
        }
        else
        {
            StartCoroutine(GoBackDelay(enemy));
        }
    }

    private IEnumerator GoBackDelay(EliteEnemy enemy)
    {
        int i = 0;
        while (i <= 20)
        {
            if (i == 20) enemy.EndAttack(enemy._spawnPosition);
            yield return _waitForFixedUpdate;
            i++;
        }
    }
}
