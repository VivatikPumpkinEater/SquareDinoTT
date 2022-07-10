using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private ParticleSystem _headShoot = null;

    [SerializeField] private int _damage = 1;
    private Rigidbody _rigidbody = null;

    private Rigidbody _rb
    {
        get => _rigidbody = _rigidbody ?? GetComponent<Rigidbody>();
    }
    
    private PoolObject _poolObj = null;
    private PoolObject _poolObject
    {
        get => _poolObj = _poolObj ?? GetComponent<PoolObject>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var enemy = collision.collider.GetComponentInParent<Enemy>();

        if (enemy)
        {
            if(collision.collider.name == "Head")
            {
                SlowMoEffects.Instance.OnSlowMotion(1f);
                
                var headShotEffect = Instantiate(_headShoot, transform.position, Quaternion.identity);
                
                headShotEffect.transform.LookAt(Camera.main.transform);
                headShotEffect.transform.rotation *= new Quaternion(-1,-1,-1, 1);
                
                Destroy(headShotEffect, 2f);
                
                enemy.Hit(Int32.MaxValue);
            }
            else
            {
                enemy.Hit(_damage);
            }
        }

        if (collision.collider.attachedRigidbody)
        {
            collision.collider.attachedRigidbody.AddForce((collision.transform.position - Camera.main.transform.position).normalized * 100, ForceMode.Impulse);
        }
        
        ResetBullet();
    }

    private void ResetBullet()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        
        _poolObject.ReturnToPool();
    }
}
