using UnityEngine;
using UnityEngine.AI;

public class RegEnemy : MonoBehaviour, ITakingDamage, IEnemy
{
    [SerializeField] private int _maxHP = 100;
    [SerializeField] private bool _onPatrol;
    [SerializeField] private bool _onAttack;
    [SerializeField] private int _damage = 10;
    [SerializeField] private AudioClip[] _audioClips = new AudioClip[6];
    [SerializeField] private GameObject _particleSystemObject;


    private Animator _animator;
    private ParticleSystem _particleSystem;
    private float _rangeAttack = 2f;
    private GameObject player;
    private int _hp;
    private Transform _spawnPosition;
    private float _spawnAngle;
    private Transform[] _patrolPoints;
    private NavMeshAgent _agent;
    private int currentPatrolPoint;
    Rigidbody _rb;
    private bool _isChangeKinematic = false;
    private AudioSource _audioSource;
    private bool _isRoar = false;
    private bool _isDead = false;
    

    public Transform SpawnPosition { get => _spawnPosition; set => _spawnPosition = value; }

    public float SpawnAngle { set => _spawnAngle = value; }

    private void Awake()
    {
        _hp = _maxHP;
        _particleSystem = _particleSystemObject.GetComponent<ParticleSystem>();
        _agent = GetComponent<NavMeshAgent>();
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _onPatrol = false;
        _onAttack = false;
        _patrolPoints = new Transform[0];
        _animator.SetBool("Walk", false);
        _animator.SetBool("Stay", true);
    }


    private void Update()
    {

        if (_isDead) return;
        if (_patrolPoints.Length > 0 && _onPatrol && !_onAttack)
        {
            _animator.SetBool("Walk", true);
            _animator.SetBool("Stay", false);
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                currentPatrolPoint = (currentPatrolPoint + 1) % _patrolPoints.Length;
                _agent.SetDestination(_patrolPoints[currentPatrolPoint].position);
            }
        }

        if (_onAttack)
        {
            _agent.SetDestination(player.transform.position);
            _animator.SetBool("Walk", true);
            _animator.SetBool("Stay", false);
            if (Vector3.Distance(player.transform.position, transform.position) <= _rangeAttack)
            {
                if (!player.GetComponent<PlayerActions>().IsDead)
                {
                    _animator.SetBool("Attack", true);
                } else
                {
                    _animator.SetBool("Attack", false);
                    EndAttack(_spawnPosition);
                }                   
            } else
            {
                _animator.SetBool("Attack", false);
            }
        }

        if (!_onAttack && !_onPatrol)
        {
            if (_agent.remainingDistance <= _agent.stoppingDistance)
            {
                _animator.SetBool("Walk", false);
                _animator.SetBool("Stay", true);
                transform.rotation = Quaternion.Euler(0f, _spawnAngle, 0f); // пока так, после сделать плавный поворот в стартовую позицию
            }
        }
    }

    private void BitePlayer()
    {
        player.GetComponent<PlayerActions>().TakingDamage(_damage, gameObject.transform);
    }
    public void StartAttack(GameObject _player)
    {
        if (_isDead) return;
        if (!_isRoar)
        {
            if(!_audioSource.isPlaying) _audioSource.PlayOneShot(_audioClips[Random.Range(0, 6)]);
            _isRoar = true;
        }
        _onAttack = true;
        player = _player;
        _agent.speed = 6;
        _agent.angularSpeed = 800;
    }

    public void EndAttack(Transform spawnPosition)
    {
        if (_isDead) return;
        if (_onAttack == true)
        {

            if (_patrolPoints.Length > 0)
            {
                _onAttack = false;
                ContinuePatrol();
                _agent.speed = 3;
            }
            else
            {
                _onAttack = false;
                _animator.SetBool("Walk", true);
                _animator.SetBool("Stay", false);
                _agent.SetDestination(_spawnPosition.position);
                _agent.speed = 3;
            }
        }
        _isRoar = false;
    }

    public void StopPatrol()
    {
        if (_isDead) return;
        if (_patrolPoints.Length > 0)
        {
            _onPatrol = false;
        }
    }

    public void ContinuePatrol()
    {
        if (_isDead) return;
        if (_patrolPoints.Length > 0)
        {
            _onPatrol = true;
            _agent.isStopped = false;
        } else
        {
            _agent.isStopped = false;
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

    public void TakingDamage(int damage, Transform sourceDamage)
    {
        _hp -= damage;
        _particleSystemObject.transform.LookAt(sourceDamage);
        _particleSystemObject.transform.position = sourceDamage.position;
        _particleSystem.Play();
        if (_hp <=0)
        {
            if (!_isDead) Death();
        }
    }

    public void TakingBombDamage(int damage)
    {
        _particleSystem.Play();
        _hp -= damage;
        if (_hp <= 0)
        {
            if(!_isDead) Death();
        }
    }

    private void Death()
    {
        _isDead = true;
        _agent.enabled = false;
        _audioSource.enabled = false;
        _animator.SetTrigger("Death");
        Destroy(gameObject, 1.5f);

    }

    public void PatrolStart(Transform[] points)
    {
        if (_isDead) return;
        _onPatrol = true;
        _patrolPoints = points;
        _agent.SetDestination(_patrolPoints[0].position);
    }

    private void ReturnKinematic()
    {
        if (_isDead) return;
        _rb.isKinematic = true;
        _isChangeKinematic = false;
    }

    public void IsBombed()
    {
        if (_isDead) return;
        _rb.isKinematic = false;
        StopPatrol();
        _agent.isStopped = true;
        _isChangeKinematic = true;
        Invoke("ContinuePatrol", 2f);
    }
}
