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
    [Range(0.1f, 1f)]
    private float _lightOffRatio;
    [SerializeField]
    private float _generateTimeOut;

    [Header("Reference")]
    [SerializeField]
    private StringVariable _gameMode;
    [SerializeField]
    private IntegerVariable _offNumber;

    [Header("Unity Events")]
    [SerializeField]
    private UnityEvent _onFinishGenLights;

    [Header("Config")]
    [SerializeField]
    private List<LED> _lights;

    public void SetupLights()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            var light = _lights[i];
            light.SetNeightbors(FindNeighbors(i));
            light.SetStatus(true);
        }
    }

    public void GenerateLightsPattern()
    {
        StartCoroutine(IE_GenerateLightsPattern());
    }

    private IEnumerator IE_GenerateLightsPattern()
    {
        float lightOffRatio = 1;
        int lightIdx;
        float timeout = _generateTimeOut;
        while (lightOffRatio > _lightOffRatio || timeout > 0f)
        {
            lightIdx = Random.Range(0, _lights.Count);
            _lights[lightIdx].Toggle();
            lightOffRatio = _offNumber.Value / _lights.Count;

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

    private void Awake()
    {
        _size = (int)Mathf.Sqrt(_lights.Count);
    }

    private void Start()
    {
        SetupLights();
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
