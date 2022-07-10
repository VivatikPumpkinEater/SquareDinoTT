using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlowZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            col.gameObject.GetComponent<NavMeshAgent>().speed = 0;
            AnimatorController.Instance.RootMotion(true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.layer.Equals(LayerMask.NameToLayer("Player")))
        {
            AnimatorController.Instance.RootMotion(false);
            col.gameObject.GetComponent<NavMeshAgent>().speed = 10;
        }
    }
}
