using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverbOff : MonoBehaviour
{
    private AudioReverbZone _audioReverbZone;

    private void Awake()
    {
        _audioReverbZone = GetComponent<AudioReverbZone>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _audioReverbZone.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _audioReverbZone.enabled = false;
        }
    }
}
