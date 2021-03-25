using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private int _size;

    [Header("Reference")]
    [SerializeField]
    private StringVariable _gameMode;

    [Header("Config")]
    [SerializeField]
    private List<LED> _lights;

    private void Awake()
    {
        _size = (int)Mathf.Sqrt(_lights.Count);
    }

    private void Start()
    {
        SetupLights();
    }

    public void SetupLights()
    {
        for (int i = 0; i < _lights.Count; i++)
        {
            _lights[i].SetNeightbors(FindNeighbors(i));
        }
    }

    private List<LED> FindNeighbors(int lightIdx)
    {
        var neighbors = new List<LED>();

        int row = lightIdx / _size;
        int column = lightIdx % _size;

        if (row - 1 >= 0)
        {
            int top = (row - 1) * _size + column;
            neighbors.Add(_lights[top]);
        }

        if (row + 1 < _size)
        {
            int bot = (row + 1) * _size + column;
            neighbors.Add(_lights[bot]);
        }

        if (column - 1 >= 0)
        {
            neighbors.Add(_lights[lightIdx - 1]);
        }

        if (column + 1 < _size)
        {
            neighbors.Add(_lights[lightIdx + 1]);
        }

        return neighbors;
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
