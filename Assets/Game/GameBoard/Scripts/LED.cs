using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LED : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private int _id;
    [SerializeField]
    private bool _isOn = true;

    [Header("Reference")]
    [SerializeField]
    private IntegerVariable _on;

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

    public void SetNeightbors(List<LED> neightbors)
    {
        _neighbors = neightbors;
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
        _isOn = !_isOn;
        _button.image.sprite = _isOn ? _onSprite : _offSprite;

        if (_isOn)
        {
            _on.Value++;
        }
        else
        {
            _on.Value--;
        }
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

}
