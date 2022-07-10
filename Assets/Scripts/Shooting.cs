using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using DG.Tweening;

public class Shooting : MonoBehaviour
{
    [SerializeField] private Transform _pewPoint = null;
    [SerializeField] private float _bulletSpeed = 25f;

    [SerializeField] private LayerMask _rayTarget;

    [SerializeField] private ChainIKConstraint _ikHand = null;
    [SerializeField] private MultiAimConstraint _ikHead = null;

    [SerializeField] private float _timeToResetWeight = 5f;

    [SerializeField] private CinemachineVirtualCamera _virtualCamera = null;

    private Transform _handTarget = null;
    private Camera _camera = null;

    private float _timer = 5f;

    private Coroutine _coroutine = null;

    private void Awake()
    {
        _camera = Camera.main;

        _ikHand.weight = 0;
        _ikHead.weight = 0;

        _handTarget = _ikHand.data.target;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameStatus.Instance.CurrentStatus == Status.Game)
        {
            _timer = _timeToResetWeight;

            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 35, _rayTarget))
            {
                if (_ikHand.weight != 1)
                {
                    CheckCoroutine();

                    _coroutine = StartCoroutine(ChangeWeight(1));
                }

                _handTarget.DOMove(hit.point, 0.1f).OnComplete(() =>
                {
                    Vector3 direction = hit.point - _pewPoint.position;
                    direction.Normalize();

                    Shot(direction);
                });
                
                _handTarget.LookAt(hit.point);
            }
        }

        if (_ikHand.weight != 0)
        {
            Timer();
        }
    }

    private void Timer()
    {
        _timer -= Time.deltaTime;

        if (_timer < 0)
        {
            _timer = _timeToResetWeight;

            CheckCoroutine();

            _coroutine = StartCoroutine(ChangeWeight(0));
        }
    }

    private void CheckCoroutine()
    {
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
    }

    private IEnumerator ChangeWeight(int value)
    {
        while (_ikHand.weight != value)
        {
            switch (value)
            {
                case 0:
                    _ikHand.weight -= 0.1f;
                    _ikHead.weight -= 0.1f;

                    _virtualCamera.m_Lens.FieldOfView -= 2;
                    break;
                case 1:
                    _virtualCamera.m_Lens.FieldOfView += 2;

                    _ikHand.weight += 0.1f;
                    _ikHead.weight += 0.1f;
                    break;
            }

            yield return new WaitForFixedUpdate();
        }

        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private void Shot(Vector3 direction)
    {
        var bullet = Pool.Instance.GetFreeElement(_pewPoint);

        var bulletRb = bullet.GetComponent<Rigidbody>();

        bulletRb.AddForce(direction * _bulletSpeed, ForceMode.VelocityChange);
    }
}