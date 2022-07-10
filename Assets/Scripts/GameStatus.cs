using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatus : MonoBehaviour
{
    public static GameStatus Instance = null;

    [SerializeField] private GameObject _startScreen = null;
    
    private Status _currentStatus = Status.preGame;

    public Status CurrentStatus
    {
        get => _currentStatus;
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

    public void SwitchStatus(Status status)
    {
        if (_currentStatus == Status.preGame && status == Status.Game)
        {
            _currentStatus = status;
            
            _startScreen.SetActive(false);
        }

        if (_currentStatus == Status.Game && status == Status.Win)
        {
            _currentStatus = status;

            StartCoroutine(ReloadScene());
        }
    }

    private IEnumerator ReloadScene()
    {
        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene(0);
    }
}

public enum Status
{
    preGame,
    Game,
    Win
}