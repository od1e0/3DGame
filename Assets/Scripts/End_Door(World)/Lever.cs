using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] private GameObject _handle;

    private Animator _handleAnimator;
    private bool _isNotRotate = true;

    private void Awake()
    {
        _handleAnimator = _handle.GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isNotRotate)
            {
                _handleAnimator.SetBool("Down", true);
                _isNotRotate = false;
                LeversManager.Instance.AddDownLeversCount();
                Debug.Log(LeversManager.Instance.DownLeversCount.ToString());
            }
        }
    }
}
