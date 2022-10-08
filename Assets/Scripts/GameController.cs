using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Converter _converter;
    private BlockController _blockController;

    private void Awake()
    {
        _converter = FindObjectOfType<Converter>();
        _blockController = FindObjectOfType<BlockController>();
        
        _converter.CreateThreeDimensionalModel();
        _blockController.SetMapReference(_converter.GetMap());
    }
}
