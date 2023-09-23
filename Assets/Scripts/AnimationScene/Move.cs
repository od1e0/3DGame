using UnityEngine;

public class Move : MonoBehaviour
{
    //Move Player
    [SerializeField] private float sensetivity;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedMult;
    [SerializeField] private float _jumpForce = 4000;
    [SerializeField] private Transform _groundDetector;
    [SerializeField] private LayerMask _groundMask;

    private Animator _animator;
    private Rigidbody _rb;
    private bool _isGrounded;
    private Vector3 _direction;
    private float _mouseLookX;
    private Vector3 _normDirection;
    private float _runMod = 1f;

    private const string Jump_str = "Jump";
    private const string Stay_str = "IsStay";
    private const string Run_str = "IsRun";
    private const string Back_str = "IsBack";
    private const string Ground_str = "OnGround";
    private const string Move_str = "MoveDirection";
    private const string Turn_str = "TurnDirection";
    private const string Side_direction_str = "SideStepDirection";
    private const string Sidestep_str = "SideStep";


    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rb = gameObject.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            _runMod = 2f;
            _animator.SetBool(Run_str, true);
        }
        if (Input.GetButtonUp("Sprint"))
        {
            _runMod = 1f;
            _animator.SetBool(Run_str, false);
        }

        if (Input.GetButton(Jump_str))
        {
            Jump();
        }

        PlayerRotate();
    }

    private void FixedUpdate()
    {
        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        _normDirection = _direction.normalized;

        MoveCheck(_direction);
        IsGroundedUpate();
        //MovementLogic(_direction * _speed);
    }


    private void MoveCheck(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            _animator.SetBool(Stay_str, false);
            _animator.SetFloat(Move_str, Mathf.Lerp(_animator.GetFloat(Move_str), _normDirection.z * _runMod, Time.deltaTime * _speed));

            _animator.SetBool(Back_str, direction.z < 0 ? true: false);

            StrafeMoveCheck(direction);
            SideMoveChek(direction);
        }
        else
        {
            SmoothEndAnimation(Move_str, Stay_str, true);
            SmoothEndAnimation(Side_direction_str, Sidestep_str, false);

            _animator.SetBool(Back_str, false);
        }
    }

    private void StrafeMoveCheck(Vector3 direction)
    {
        if (direction.z != 0 && direction.x != 0)
        {
            _animator.SetFloat(Turn_str, Mathf.Lerp(_animator.GetFloat(Turn_str), _normDirection.x * _runMod, Time.deltaTime * _speed));
        }
        else if (direction.x == 0)
        {
            SmoothEndAnimation(Turn_str);
        }
    }

    private void SideMoveChek(Vector3 direction)
    {
        if (direction.z == 0 && direction.x != 0)
        {
            _animator.SetBool(Sidestep_str, true);
            _animator.SetFloat(Side_direction_str, Mathf.Lerp(_animator.GetFloat(Side_direction_str), _normDirection.x * _runMod, Time.deltaTime * _speed));
        } else if (direction.z != 0)
        {
            SmoothEndAnimation(Side_direction_str);
            _animator.SetBool(Sidestep_str, false);
        }
    }

    private void PlayerRotate()
    {
        _mouseLookX = Input.GetAxis("Mouse X") * sensetivity * Time.deltaTime;

        if(_mouseLookX != 0 && _animator.GetBool(Stay_str))
        {
            _animator.SetFloat(Turn_str, Mathf.Lerp(_animator.GetFloat(Turn_str), (_mouseLookX > 0) ? 1 : -1, Time.deltaTime * _speed));
            
        } else if (_animator.GetBool(Stay_str))
        {
            SmoothEndAnimation(Turn_str);
        }

        if (!_animator.GetBool(Stay_str)) // Если двигаемся, то разрешаем менять направление мышкой
            transform.Rotate(0, _mouseLookX, 0);
    }


    //private void MovementLogic(Vector3 speed)
    //{
    //    _rb.AddForce(transform.forward * speed.z, ForceMode.Impulse);
    //    _rb.AddForce(transform.right * speed.x, ForceMode.Impulse);

    //}

    private void Jump()
    {
        if (_isGrounded)
        {
            _animator.SetBool(Jump_str, true);
            _animator.SetBool(Ground_str, false);
            if (_animator.GetBool(Run_str) && !_animator.GetBool(Back_str)) _rb.AddForce(Vector3.up * _jumpForce);
        }
    }

    private void IsGroundedUpate()
    {
        _isGrounded = Physics.CheckSphere(_groundDetector.position, 0.2f, _groundMask);

        if (_isGrounded)
        {
            _animator.SetBool(Jump_str, false);
            _animator.SetBool(Ground_str, true);
        }
    }

    private void SmoothEndAnimation(string firstParamName)
    {
        if (_animator.GetFloat(firstParamName) > 0.1f || _animator.GetFloat(firstParamName) < -0.1f)
        {
            _animator.SetFloat(firstParamName, Mathf.Lerp(_animator.GetFloat(firstParamName), 0, Time.deltaTime * _speed));
        }
        else
        {
            _animator.SetFloat(firstParamName, 0);
        }
    }

    private void SmoothEndAnimation(string firstParamName, string secondParamName, bool paramFlag)
    {
        if (_animator.GetFloat(firstParamName) > 0.1f || _animator.GetFloat(firstParamName) < -0.1f)
        {
            _animator.SetFloat(firstParamName, Mathf.Lerp(_animator.GetFloat(firstParamName), 0, Time.deltaTime * _speed));
        }
        else
        {
            _animator.SetFloat(firstParamName, 0);
            _animator.SetBool(secondParamName, paramFlag);
        }
    }
}
