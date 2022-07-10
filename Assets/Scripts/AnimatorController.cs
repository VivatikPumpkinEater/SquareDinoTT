using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    public static AnimatorController Instance = null;

    private Animator _animator = null;

    private Animator _anim
    {
        get => _animator = _animator ?? GetComponent<Animator>();
    }
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }

        Instance = this;
    }

    public void ActivatedTrigger(string trigger)
    {
        _anim.SetTrigger(trigger);
    }

    public void FloatChanger(string name, float value)
    {
        _anim.SetFloat(name, value);
    }

    public void RootMotion(bool enable)
    {
        _anim.applyRootMotion = enable;
    }
}
