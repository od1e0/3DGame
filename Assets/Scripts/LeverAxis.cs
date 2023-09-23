using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverAxis : MonoBehaviour
{
    [SerializeField] private GameObject _destroibleObject;

    private Animator animator;
    private bool _isNotRotate = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_isNotRotate)
            {
                //transform.rotation = Quaternion.Euler(135, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z); // —делать бы плавное опускание рычага, узнать как.
                animator.SetBool("Down", true);
                other.GetComponent<PlayerActions>().AddLeverCount();
                _isNotRotate = false;
                
                if (other.GetComponent<PlayerActions>().LeverCount >= 4)
                {
                    _destroibleObject.SetActive(false);
                    Debug.Log("Secret Door is open");
                }
            }
        }
    }
}
