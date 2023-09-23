using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Debug.Log(other.tag);
            _animator.SetBool("needOpen", true);
            _animator.SetBool("needClose", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Player"))
        {
            Debug.Log(other.tag);
            _animator.SetBool("needOpen", false);
            _animator.SetBool("needClose", true);
        }
    }
}
