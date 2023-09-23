using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private bool _onEdge = false;
    private MeshRenderer _mr;

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if (_onEdge)
        {
            _mr.material.color = Color.red;
        } else
        {
            _mr.material.color = Color.green;
        }
    }

    public void TargetIsNear(bool check)
    {
        _onEdge = check;
    }
}
