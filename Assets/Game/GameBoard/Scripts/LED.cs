using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LED : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private bool _isOn = true;

    [Header("Reference")]
    [SerializeField]
    private IntegerVariable _onNumber;
    [SerializeField]
    private IntegerVariable _offNumber;

    [Header("Runtime Reference")]
    [SerializeField]
    private List<LED> _neighbors;

    [Header("Config")]
    [SerializeField]
    private Sprite _onSprite;
    [SerializeField]
    private Sprite _offSprite;
    [SerializeField]
    private Button _button;

    private GameBoard _parrent;

    public void SetParrent(GameBoard parrent)
    {
        _parrent = parrent;
    }

    public void SetNeightbors(List<LED> neightbors)
    {
        _neighbors = neightbors;
    }

    public void SetStatus(bool turnOn)
    {
        _isOn = turnOn;

        _button.image.sprite = _isOn ? _onSprite : _offSprite;

        if (_isOn)
        {
            _onNumber.Value++;
            _offNumber.Value--;
        }
        else
        {
            _onNumber.Value--;
            _offNumber.Value++;
        }

        NotifyToParrent(turnOn);
    }

    public void RemoveNeighbor()
    {
        _neighbors.Clear();
    }

    public void Toggle()
    {
        Flip();
        FlipNeighbors();
    }

    public void Flip()
    {
        SetStatus(!_isOn);   
    }

    private void FlipNeighbors()
    {
        if (_neighbors == null)
        {
            return;
        }

        for (int i = 0; i < _neighbors.Count; i++)
        {
            _neighbors[i].Flip();
        }
    }

    private void NotifyToParrent(bool isOn)
    {
        _parrent.UpdateLightStatus(isOn);
    }
}
