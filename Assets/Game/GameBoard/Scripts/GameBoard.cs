using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameBoard : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private int _size;
    [SerializeField]
    private int _on;
    [SerializeField]
    private int _off;

    [Header("Reference")]
    [SerializeField]
    private IntegerVariable _offNumber; // Referee only relies on this value;
    [SerializeField]
    private IntegerVariable _onNumber; // Referee only relies on this value;

    [Header("Events in")]
    [SerializeField]
    private GameEvent _onRequestNewGame;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onStartGenLights;
    [SerializeField]
    private UnityEvent _onFinishGenLights;

    [Header("Config")]
    [SerializeField]
    [Range(0.1f, 1f)]
    private float _configLightOffRatio;
    [SerializeField]
    private float _generateTimeOut;
    [SerializeField]
    private List<LED> _lights;

    private UIPanel _uiPanel;

    public void SetupLights()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            var light = _lights[i];
            light.SetParrent(this);
            light.SetNeightbors(FindNeighbors(i));
            light.SetStatus(true);
        }

        _on = _lights.Count;
        _off = 0;
    }

    public void RegisterCurrentOnOffToReferee()
    {
        _onNumber.Value = _on;
        _offNumber.Value = _off;
    }

    public void GenerateLightsPattern()
    {
        RegisterCurrentOnOffToReferee();
        StartCoroutine(IE_GenerateLightsPattern());
    }

    public void UpdateLightStatus(bool isOn)
    {
        if (!isOn)
        {
            _on--;
            _off++;
        }
        else
        {
            _on++;
            _off--;
        }

        if (_on > 0)
        {
            return;
        }

        ResetAllLights();
    }

    private void ResetAllLights()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i].SetStatus(true);
        }
    }

    private IEnumerator IE_GenerateLightsPattern()
    {
        _onStartGenLights.Invoke();

        float lightOffRatio = 0;
        int lightIdx;
        float timeout = _generateTimeOut;
        while (lightOffRatio < _configLightOffRatio)
        {
            if (timeout < 0f)
            {
                break;
            }

            lightIdx = Random.Range(0, _lights.Count);
            _lights[lightIdx].Toggle();
            lightOffRatio = _off / _lights.Count;

            timeout -= Time.deltaTime;
            yield return null;
        }

        _onFinishGenLights.Invoke();
    }

    private List<LED> FindNeighbors(int lightIdx)
    {
        var neighbors = new List<LED>();

        int column = lightIdx % _size;

        // top neighbor
        if (lightIdx - _size >= 0)
        {
            neighbors.Add(_lights[lightIdx - _size]);
        }

        // bot neighbor
        if (lightIdx + _size < _lights.Count)
        {
            neighbors.Add(_lights[lightIdx + _size]);
        }

        // left neighbor
        if (column - 1 >= 0)
        {
            neighbors.Add(_lights[lightIdx - 1]);
        }

        // right neighbor
        if (column + 1 < _size)
        {
            neighbors.Add(_lights[lightIdx + 1]);
        }

        return neighbors;
    }

    private void HandleNewGameRequest(params object[] args)
    {
        if (!_uiPanel.IsOpening)
        {
            return;
        }

        GenerateLightsPattern();
    }

    private void Awake()
    {
        _uiPanel = GetComponent<UIPanel>();
        _size = (int)Mathf.Sqrt(_lights.Count);
        _onRequestNewGame.Subcribe(HandleNewGameRequest);
    }

    private void Start()
    {
        SetupLights();
    }

    private void OnDestroy()
    {
        _onRequestNewGame.Unsubcribe(HandleNewGameRequest);
    }

#if UNITY_EDITOR
    public void RemoveLightNeighbor()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i].RemoveNeighbor();
        }
    }    
#endif
}
