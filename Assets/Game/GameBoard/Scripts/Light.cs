using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light : MonoBehaviour
{
    [Header("Inspec")]
    [SerializeField]
    private int _id;

    [Header("Reference")]
    [SerializeField]
    private StringVariable _gameMode;

    [Header("Runtime Reference")]
    [SerializeField]
    private List<Light> _neightbors;

    [Header("Config")]
    [SerializeField]
    private Sprite _onSprite;
    [SerializeField]
    private Sprite _offSprite;
}
