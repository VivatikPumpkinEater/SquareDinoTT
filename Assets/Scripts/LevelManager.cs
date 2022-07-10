using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance = null;

    [SerializeField] private List<Places> _places = new List<Places>();

    [SerializeField] private NavMeshAgent _meshAgent = null;

    private int _stage = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }

        Instance = this;

        SubscribeOnDie();
    }

    private void SubscribeOnDie()
    {
        foreach (var stages in _places)
        {
            stages.Positions.Complete += StageDone;
            foreach (var enemies in stages.Enemies)
            {
                enemies.Die += RemoveEnemies;
            }
        }
    }

    private void StartGame()
    {
        GameStatus.Instance.SwitchStatus(Status.Game);
        
        _meshAgent.SetDestination(_places[_stage].Positions.transform.position);
    }

    private void FindEnemy()
    {
        if (_places[_stage].Enemies != null)
        {
            float minDistance = Single.MaxValue;
            Vector3 minPos = Vector3.positiveInfinity;

            foreach (var enemy in _places[_stage].Enemies)
            {
                if (Vector3.Distance(enemy.transform.position, _meshAgent.transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(enemy.transform.position, _meshAgent.transform.position);
                    minPos = enemy.transform.position;
                }
            }

            _meshAgent.transform.DOLookAt(minPos, 1f);
        }
    }

    private void StageDone()
    {
        _places[_stage].Positions.Complete -= StageDone;

        MoveNext();
    }

    private void MoveNext()
    {
        if (_places[_stage].Enemies.Count == 0)
        {
            if (_stage < _places.Count - 1)
            {
                _stage++;
            }
            else
            {
                GameStatus.Instance.SwitchStatus(Status.Win);
                
                AnimatorController.Instance.ActivatedTrigger("Dance");
            }

            _meshAgent.SetDestination(_places[_stage].Positions.transform.position);
        }
        else
        {
            FindEnemy();
        }
    }

    private void Update()
    {
        AnimatorController.Instance.FloatChanger("Movement", _meshAgent.velocity.magnitude);

        if (Input.GetMouseButtonDown(0) && GameStatus.Instance.CurrentStatus == Status.preGame)
        {
            StartGame();
        }
    }

    private void RemoveEnemies(Enemy enemy)
    {
        foreach (var stages in _places)
        {
            foreach (var enemies in stages.Enemies)
            {
                if (enemies.Equals(enemy))
                {
                    stages.Enemies.Remove(enemies);

                    enemies.Die -= RemoveEnemies;
                    break;
                }
            }
        }

        MoveNext();
    }
}

[System.Serializable]
public struct Places
{
    public CompleteTrigger Positions;
    public List<Enemy> Enemies;
}