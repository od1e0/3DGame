using UnityEngine;
using UnityEngine.AI;

public class EliteEnemy : MonoBehaviour, ITakingDamage, IEnemy
{
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private bool _onAttack;
    [SerializeField] private int _damage = 10;
    [SerializeField] public Transform _spawnPosition;
    [SerializeField] private Color _color;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _tooltipe;
    [SerializeField] private AudioClip _attackRoar;
    [SerializeField] private GameObject _particleSystemObject;

    private Animator _animator;
    private bool _isDead = false;
    private ParticleSystem _particleSystem;
    private AudioSource _audioSource;
    public bool _isCanTakeDamage;
    private float _rangeAttack = 2f;   
    private int _hp;
    private NavMeshAgent _agent;
    Rigidbody _rb;
    private bool _isChangeKinematic = false;
    private bool _isAttackRoar = false;


    private void Awake()
    {
        _particleSystem = _particleSystemObject.GetComponent<ParticleSystem>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _hp = _maxHP;
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _onAttack = false;
        _isCanTakeDamage = false;
        _animator.SetBool("Walk", false);
        _animator.SetBool("Stay", true);
    }


    private void Update()
    {
        if (_isDead) return;
        if (_onAttack)
        {
            _agent.SetDestination(_player.transform.position);
            _animator.SetBool("Walk", true);
            _animator.SetBool("Stay", false);
            if (Vector3.Distance(_player.transform.position, transform.position) <= _rangeAttack)
            {
                if (!_player.GetComponent<PlayerActions>().IsDead)
                {
                    _animator.SetBool("Attack", true);
                }
                else
                {
                    _animator.SetBool("Attack", false);
                    EndAttack(_spawnPosition);
                }
            }
            else
            {
                _animator.SetBool("Attack", false);
            }
        } else
        {
            _animator.SetBool("Attack", false);
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("Stay", true);
                transform.rotation = Quaternion.Euler(0f, _spawnPosition.rotation.eulerAngles.y, 0f); // пока так, после сделать плавный поворот в стартовую позицию
            }

        }
    }

    private void BitePlayer()
    {
        _player.GetComponent<PlayerActions>().TakingDamage(_damage, gameObject.transform);
    }

    public void StartAttack()
    {
        if (_isDead) return;
        if (!_isAttackRoar)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.Stop();
                _audioSource.PlayOneShot(_attackRoar);
            }
            _isAttackRoar = true;
        }
        _onAttack = true;
    }

    public void EndAttack(Transform spawnPosition)
    {
        if (_isDead) return;
        if (_onAttack == true)
        {
            _onAttack = false;
            _agent.SetDestination(_spawnPosition.position);
            _animator.SetBool("Walk", true);
            _animator.SetBool("Stay", false);
        }
        _isAttackRoar = false;
    }


    private void FixedUpdate()
    {
        if (_isChangeKinematic == true)
        {
            _isChangeKinematic = false;
            Invoke("ReturnKinematic", 2f);
        }
    }

    public void TakingDamage(int damage, Transform sourceDamage)
    {
        if (_isCanTakeDamage)
        {
            _hp -= damage;
            _particleSystemObject.transform.LookAt(sourceDamage);
            _particleSystemObject.transform.position = sourceDamage.position;
            _particleSystem.Play();
            if (_hp <= 0)
            {
                if (!_isDead) Death();
            }
        }
    }

    public void TakingBombDamage(int damage)
    {
        _hp -= damage;
        _particleSystem.Play();
        if (_hp <= 0)
        {
            if (!_isDead) Death();
        }
    }

    private void Death()
    {
        _isDead = true;
        _player.GetComponent<PlayerActions>().AddKey(_color);
        _agent.enabled = false;
        _audioSource.enabled = false;
        _animator.SetTrigger("Death");

        if (PlayerPrefs.GetInt("_isShowTooltip") != 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            _tooltipe.SetActive(true);
            AudioListener.volume = 0;
            PlayerPrefs.SetInt("_isShowTooltip", 1);
        }
        Destroy(gameObject, 1.5f);
    }


    private void ReturnKinematic()
    {
        if (_isDead) return;
        _rb.isKinematic = true;
        _isChangeKinematic = false;
        _agent.isStopped = false;
    }

    public void IsBombed()
    {
        if (_isDead) return;
        _rb.isKinematic = false;
        _agent.isStopped = true;
        _isChangeKinematic = true;
        Invoke("ReturnKinematic", 1f);
    }
}
