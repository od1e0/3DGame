using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : MonoBehaviour
{
    [SerializeField] private int _healCount = 10;

    private void Update()
    {
        //transform.Rotate(0, 1, 0);
        //transform.position = new Vector3(transform.position.x, 1f + Mathf.Sin(Time.fixedTime * 3f) * 0.2f, transform.position.z);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerActions>().GetHeal(_healCount);
            Debug.Log("GetHeal!!");
            Destroy(gameObject);
        }
    }
}
