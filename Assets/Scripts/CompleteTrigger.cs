using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteTrigger : MonoBehaviour
{
    public System.Action Complete;
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            Complete?.Invoke();
        }
    }
}
