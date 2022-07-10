using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Material _dieMaterial = null;

    [SerializeField] private int _health = 3;
    [SerializeField] private Image _healthBar = null;
    
    [SerializeField] private List<Rigidbody> _bones = new List<Rigidbody>();

    public System.Action<Enemy> Die;

    private float _stepOneHp = 0;

    private Animator _animator = null;

    private Animator _anim
    {
        get => _animator = _animator ?? GetComponent<Animator>();
    }

    private SkinnedMeshRenderer _skinnedMeshRenderer = null;

    private SkinnedMeshRenderer _skinRenderer
    {
        get => _skinnedMeshRenderer = _skinnedMeshRenderer ?? GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private bool _die = false;

    [ContextMenu("Fill list")]
    private void FillList()
    {
        var bones = GetComponentsInChildren<Rigidbody>();

        foreach (var bone in bones)
        {
            bone.isKinematic = true;
            _bones.Add(bone);
        }
    }

    private void Awake()
    {
        _stepOneHp = _healthBar.fillAmount / _health;
    }

    public void Hit(int damage)
    {
        _health -= damage;

        _healthBar.fillAmount -= _stepOneHp;

        if (_health <= 0)
        {
            _healthBar.fillAmount = 0;
            Dead();
        }
    }

    private void Dead()
    {
        if (!_die)
        {
            _die = true;
            
            _skinRenderer.material = _dieMaterial;

            Die?.Invoke(this);

            _anim.enabled = false;

            foreach (var bone in _bones)
            {
                bone.isKinematic = false;
            }
        }
    }
    
}