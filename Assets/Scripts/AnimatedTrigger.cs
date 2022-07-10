using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimatedTrigger : MonoBehaviour
{
    [SerializeField] private string _animationTrigger = String.Empty;

    private bool _complete = false;

    private void OnTriggerEnter(Collider col)
    {
        if (!_complete)
        {
            if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
            {
                _complete = true;
                
                Debug.Log("Trigger");
                
                AnimatorController.Instance.ActivatedTrigger(_animationTrigger);
            }
        }
    }
    
}