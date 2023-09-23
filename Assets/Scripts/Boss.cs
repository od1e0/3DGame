using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour,ITakingDamage, IEnemy
{

    [SerializeField] private int _maxHP = 300;
    [SerializeField] private bool _onAttack;
    [SerializeField] private int _damage = 60;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private bool _isSecret;
    [SerializeField] private bool _isMain;
    [SerializeField] private AudioClip[] _clips = new AudioClip[6];
    [SerializeField] private AudioClip _attackRoar;
    [SerializeField] private GameObject _particleSystemObject;
    [SerializeField] private GameObject _endGamePanel;

    private Animator _animator;
    private ParticleSystem _particleSystem;
    private float _rangeAttack = 2f;
    private GameObject player;
    private int _hp;
    private NavMeshAgent _agent;
    private bool _isChangeKinematic = false;
    private Rigidbody _rb;
    private AudioSource _audioSource;
    private bool _isRoared = false;
    private bool _isAttackRoar = false;
    private bool _isDead = false;




    private void Awake()
    {
        _particleSystem = _particleSystemObject.GetComponent<ParticleSystem>();
        _hp = _maxHP;
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _onAttack = false;
        _audioSource = GetComponent<AudioSource>();
        _animator.SetBool("Fly", true);
    }


    private void Update()
    {
        if (_isDead) return;
        if (_onAttack)
        {
            _agent.SetDestination(player.transform.position);
            if (Vector3.Distance(player.transform.position, transform.position) <= _rangeAttack)
            {
                _animator.SetBool("Attack", true);
                _animator.SetBool("Fly", false);
            }
            else
            {
                _animator.SetBool("Attack", false);
                _animator.SetBool("Fly", true);
            }
        } else
        {
            if (!_isRoared)
            {
                Invoke("BossRoar", Random.Range(10f, 20f));
                _isRoared = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isChangeKinematic == true)
        {
            _isChangeKinematic = false;
            Invoke("ReturnKinematic", 2f);
        }
    }

    private void BitePlayer()
    {
        if (_isDead) return;
        player.GetComponent<PlayerActions>().TakingDamage(_damage, gameObject.transform);
    }

    public void Attack(GameObject _player)
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
        player = _player;
        
    }

    public void EndAttack()
    {
        if (_isDead) return;
        if (_onAttack == true)
        {
            _animator.SetBool("Attack", false);
            _animator.SetBool("Fly", true);
            _onAttack = false;
            _agent.SetDestination(_startPosition.position);
            if (_agent.remainingDistance <= _agent.stoppingDistance)
                transform.rotation = Quaternion.Euler(0f, _startPosition.rotation.eulerAngles.y, 0f); // пока так, после сделать плавный поворот в стартовую позицию
        }

        _isAttackRoar = false;
    }

    public void TakingDamage(int damage, Transform sourceDamage)
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
        _agent.enabled = false;
        _audioSource.enabled = false;
        _animator.SetTrigger("Death");
        if (_isSecret)
        {
            player.GetComponent<PlayerActions>().SecretBossDamageModifer = 3;
            Destroy(gameObject, 1.5f);
        }

        if (_isMain)
        {
            Cursor.lockState = CursorLockMode.None;
            _endGamePanel.SetActive(true);
            Destroy(gameObject, 1.5f);
        }
        
    }

    public void IsBombed()
    {
        if (_isDead) return;
        _rb.isKinematic = false;
        _isChangeKinematic = true;
    }

    private void ReturnKinematic()
    {
        if (_isDead) return;
        _rb.isKinematic = true;
        _isChangeKinematic = false;
    }

    private void BossRoar()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.PlayOneShot(_clips[Random.Range(0,6)]);
        }
        _isRoared = false;
    }
}

