using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

public class Referee : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField]
    private IntegerVariable _lightOff;
    [SerializeField]
    private IntegerVariable _lightOn;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onAllLightsOff;

    public void Quit()
    {
        Application.Quit();
    }

    private void TrackLightOnNumber(int number)
    {
        if (number > 0)
        {
            return;
        }

        _onAllLightsOff.Invoke();
    }

    private void Start()
    {
        Observable.TimerFrame(2).Subscribe(_ =>
        {
            _lightOn.OnValueChange += TrackLightOnNumber;
        });
    }

    private void OnDestroy()
    {
        _lightOn.OnValueChange -= TrackLightOnNumber;
    }
}
