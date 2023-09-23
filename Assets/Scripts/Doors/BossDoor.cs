using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoor : MonoBehaviour
{
    [SerializeField] GameObject[] pedestals = new GameObject[4];
    Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    void Update()
    {
        int lightCount = 0;

        foreach (var item in pedestals)
        {
            if (item.GetComponent<Pedestal>()._onLight) lightCount++;
        }
        if (lightCount == 4) animator.SetBool("AllLightOn", true);
    }
}
