using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int _damage = 50;
    [SerializeField] private float _activationTime = 3f;
    [SerializeField] private GameObject _indicator;
    [SerializeField] private GameObject _particleObject;
    [SerializeField] private GameObject _mineBody;
    private bool _isPressed = false;
    private bool _isBombed = false;
    private float _stayTime = 0f;

    private void OnTriggerStay(Collider other)
    {
        if (_isBombed) return;
        if (other.tag.Equals("Player"))
        {

            Debug.Log("Mine is near");
            _indicator.GetComponent<Indicator>().TargetIsNear(true);

            _stayTime += Time.deltaTime;

            if (_stayTime >= _activationTime)
            {
                _stayTime = 0f;
                other.gameObject.GetComponent<PlayerActions>().TakingDamage(_damage, gameObject.transform);
                _particleObject.SetActive(true);
                GetComponent<AudioSource>().Play();
                Destroy(_mineBody);
                _isBombed = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(gameObject, 5f);
            }

            if (_isPressed)
            {
                _particleObject.SetActive(true);
                GetComponent<AudioSource>().Play();
                Destroy(_mineBody);
                _isBombed = true;
                gameObject.GetComponent<BoxCollider>().enabled = false;
                Destroy(gameObject, 5f);
                _isPressed = false;
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_isBombed) return;
        _indicator.GetComponent<Indicator>().TargetIsNear(false);
        _stayTime = 0f;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            collision.gameObject.GetComponent<PlayerActions>().TakingDamage(_damage, gameObject.transform);
            _isPressed = true;
        }
    }
}
