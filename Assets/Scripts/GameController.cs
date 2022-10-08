using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Converter _converter;

    private void Awake()
    {
        _converter = FindObjectOfType<Converter>();

        _converter.CreateThreeDimensionalModel();
    }
}
