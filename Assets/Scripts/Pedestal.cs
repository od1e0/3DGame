using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedestal : MonoBehaviour
{
    [SerializeField] GameObject _sphere;
    [SerializeField] string _pcolor;
    public bool _onLight = false;

    MeshRenderer _mr;
    Color _color;
    

    private void Awake()
    {
        _mr = GetComponent<MeshRenderer>();
        _color = _mr.material.color;
    }
    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerActions>().KeyContainer[_color] == 1)
            {
                _sphere.GetComponent<Sphere>().ChangeColor(_pcolor);
                //_sphere.GetComponent<MeshRenderer>().material.color = _color;
                _onLight = true;

            }
        }
    }
}
