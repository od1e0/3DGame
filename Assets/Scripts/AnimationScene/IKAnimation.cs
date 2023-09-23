using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class IKAnimation : MonoBehaviour
{
    [SerializeField] private bool _isActive;
    [SerializeField] private Transform _catchObject;
    [SerializeField] private Transform _catchPoint;
    [SerializeField] private LayerMask _rayLayer;

    private Dictionary<Transform, float> _lookObjectContainer = new Dictionary<Transform, float>();
    private GameObject _catchPointForWall;
    private Transform _lookObject;
    private Animator _animator;
    private float _speed = 0.5f;
    private float _weight;
    private bool _isInVision;
    private bool _isWallNear;
    private bool _isTargeted;

    private const string LookObjectTag = "InteractiveLook";
    private const string CatchObjectTag = "InteractiveCatch";
    private const string WallsTag = "Walls";
    private const float HalfVector3Modifer = 0.5f;


    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //CheckIK(); //ѕо хорошему разгрузить апдейт и убрать метод из него, флаг активации поднимать и опускать по триггеру,
        //а вес считать только если игрок остановилс€ в OnAnimatorIK. “огда не будет лишних просчетов, которые никак не используютс€.
        if (_lookObjectContainer.Count > 0)
        {
            _lookObject = GetObject(_lookObjectContainer);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        if (!_isActive) return;
        
        if(_lookObject != null)
        {
            _animator.SetLookAtWeight(_weight);
            _animator.SetLookAtPosition(_lookObject.position);
        }

        if(IsObjectNear(_catchObject) && _isInVision)
        {
            //float angle = Vector3.Angle(_catchPoint.position, transform.forward); // ’отел ограничивать углы, но пон€л что смотреть на угол между объектом
            //Debug.Log(angle);                                                     // и игроком плоха€ зате€. Ќужно смотреть на углы поворота костей модели и
            // ограничивать именно их.

            SetIKAnimator(_catchPoint);
        }

        if (!_isWallNear) return;
  
        Vector3 _leftHand = _animator.GetIKPosition(AvatarIKGoal.LeftHand);
        if (Physics.Raycast(_leftHand + (Vector3.up * HalfVector3Modifer), Vector3.forward, out var hit, 0.8f, _rayLayer))
        {
            if (!_isTargeted)
            {
                _catchPointForWall = new GameObject();
                _catchPointForWall.transform.Translate(hit.point);
                _catchPointForWall.transform.Rotate(-103f, 0, 0);

                _isTargeted = true;
            }

            SetIKAnimator(_catchPointForWall.transform);
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckIK();

        switch (other.tag)
        {
            case LookObjectTag: 
                _lookObjectContainer.Add(other.transform, Vector3.Distance(other.transform.position, transform.position));
                break;

            case CatchObjectTag: 
                _isInVision = true;
                break;

            case WallsTag:
                _isWallNear = true;
                break;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        CheckIK();

        if (other.CompareTag(LookObjectTag))
        {
            UpdateDistanceValue(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        switch (other.tag)
        {
            case LookObjectTag:
                _lookObjectContainer.Remove(other.transform);
                if (_lookObjectContainer.Count < 1) _lookObject = null;
                break;

            case CatchObjectTag:
                _isInVision = false;
                break;

            case WallsTag:
                _isWallNear = false;
                break;
        }

        CheckIK();
    }

    private void SetIKAnimator(Transform _catchPoint)
    {
        _animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, _weight);
        _animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, _weight);
        _animator.SetIKPosition(AvatarIKGoal.LeftHand, _catchPoint.position);
        _animator.SetIKRotation(AvatarIKGoal.LeftHand, _catchPoint.rotation);
    }

    private void UpdateDistanceValue(Collider other)
    {
        if (_lookObjectContainer.TryGetValue(other.transform, out float value))
        {
            _lookObjectContainer[other.transform] = Vector3.Distance(other.transform.position, transform.position);
        }
    }

    private Transform GetObject(Dictionary<Transform, float> _objectContainer) => _objectContainer.OrderBy(x => x.Value)
                                                                                                .ToDictionary(x => x.Key, x => x.Value)
                                                                                                .First().Key;

    private bool IsObjectNear(Transform catchObject)
    {
        float distance = Vector3.Distance(catchObject.transform.position, transform.position);

        return distance > 0.4f && distance < 1.5f;
    }

    private void CheckIK()
    {
        if (_animator.GetBool("IsStay"))
        {
            _isActive = true;
            _weight = Mathf.Lerp(_weight, 3, Time.deltaTime * _speed);
        }
        else
        {
            _isActive = false;
            _weight = 0;

            if (_isTargeted) //удаление объекта создаваемого дл€ касани€ стены
            {
                Destroy(_catchPointForWall);
                _isTargeted = false;
            }
        }
    }
}
