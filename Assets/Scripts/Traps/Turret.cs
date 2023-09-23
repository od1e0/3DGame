using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _base;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject _bulletPref;
    [SerializeField] private Transform _bulletStartPosition;
    [SerializeField] public float _fireRate = 0.8f;
    [SerializeField] public AudioClip _shotClip;


    private float _shotTimer = 0f;
    private float _rotationDelay = 0f;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.enabled = false;
            Quaternion rotateTarget = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, _target.position + (Vector3.up * 1.4f) - transform.position, _speed * Time.deltaTime, 0.0f));
            transform.rotation = new Quaternion(0, rotateTarget.y, 0, rotateTarget.w);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator.enabled = false;
            Quaternion rotateTarget = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, _target.position + (Vector3.up * 1.4f) - transform.position, _speed * Time.deltaTime, 0.0f));
            PlayerSearch(rotateTarget);
            if (_shotTimer == 0f && _rotationDelay > 0.8f) Fire();
            _shotTimer += Time.deltaTime;
            _rotationDelay += Time.deltaTime;
            if (_shotTimer > _fireRate)
            {
                _shotTimer = 0;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        animator.enabled = true;
        //transform.rotation = new Quaternion(0, 0, 0, 0); // Нужно сделать плавный поворот в начальную позицию, пока не придумал как
        //_shotTimer = 0f;
        //_rotationDelay = 0f;
    }

    private void PlayerSearch(Quaternion rotateTarget)
    {
        rotateTarget = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, _target.position + (Vector3.up * 1.4f) - transform.position, _speed * Time.deltaTime, 0.0f));

        //Первая попытка ограничить углы поворота, с учетом изменения угла базы, расчитывались относительно поворота базы по оси Y, учитывалось что при смене координат базы
        //нужны были проверки угла как с логическим И, так и с ИЛИ. Пытался переделать добавив ограничивающие объекты, но при повороте родительского объекта приходилось делать
        //более сложные условия. Возможно лучше будет переделать работу с коллайдерами и избавиться от текущих условий. Пока так
        if (_base.rotation.eulerAngles.y > 90)
        {
            if ((rotateTarget.eulerAngles.x <= 40 || rotateTarget.eulerAngles.x >= 320) && rotateTarget.eulerAngles.y >= GetMax(270) && rotateTarget.eulerAngles.y <= GetMax(90))
                transform.rotation = rotateTarget;
        }
        else if (_base.rotation.eulerAngles.y <= 90) //Если угол поворота базы больше 90 градусов, переходим на конструкцию с логическим ИЛИ
        {
            if ((rotateTarget.eulerAngles.x <= 40 || rotateTarget.eulerAngles.x >= 320) && (rotateTarget.eulerAngles.y >= GetMax(270) || rotateTarget.eulerAngles.y <= GetMax(90)))
                transform.rotation = rotateTarget;
        }
    }

    public void Fire()
    {
        var bullet = Instantiate(_bulletPref, _bulletStartPosition.position, transform.rotation);
        GetComponent<AudioSource>().PlayOneShot(_shotClip);
    }

    //Функция расчета максимального отклонения.
    private float GetMax(float maxRotation)
    {
        if (_base.rotation.eulerAngles.y + maxRotation > 360f) return (_base.rotation.eulerAngles.y + maxRotation) - 360f;
        else return _base.rotation.eulerAngles.y + maxRotation;
    }
}
