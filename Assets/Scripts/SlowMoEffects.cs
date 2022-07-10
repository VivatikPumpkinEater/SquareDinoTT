using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMoEffects : MonoBehaviour
{
    public static SlowMoEffects Instance = null;
    
    [SerializeField] private int _timeScaling = 4;

    private SlowMoStatus _status = SlowMoStatus.Off;
    
    private float _slowMoValue = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
            Instance = null;
        }

        Instance = this;
        
        _slowMoValue = 1f / _timeScaling;

        Debug.Log(Application.platform);
    }
    

    public void OnSlowMotion(float duration)
    {
        if(_status != SlowMoStatus.On)
        {
            Time.timeScale = _slowMoValue;
            Time.fixedDeltaTime *= _slowMoValue;
            
            _status = SlowMoStatus.On;

            StartCoroutine(SlowMoTimer(duration));
        }
    }

    private IEnumerator SlowMoTimer(float duration)
    {
        yield return new WaitForSecondsRealtime(duration);
        
        DisableSlowMotion();
    }

    private void DisableSlowMotion()
    {
        if(_status != SlowMoStatus.Off)
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime /= _slowMoValue;

            _status = SlowMoStatus.Off;
        }
    }
}

public enum SlowMoStatus
{
    On,
    Off
}
