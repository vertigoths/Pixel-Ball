using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Converter _converter;
    private LevelController _levelController;
    private BallSpawner _ballSpawner;
    private BlockController _blockController;
    
    private bool _canCreate = true;

    private void Awake()
    {
        _converter = FindObjectOfType<Converter>();
        _levelController = FindObjectOfType<LevelController>();
        _ballSpawner = FindObjectOfType<BallSpawner>();
        _blockController = FindObjectOfType<BlockController>();
    }

    private void Start()
    {
        StartCoroutine(StartGame(1f));
    }

    public IEnumerator StartGame(float delayTime)
    {
        if (_canCreate)
        {
            _canCreate = false;
            
            yield return new WaitForSeconds(delayTime);
        
            _converter.CreateThreeDimensionalModel();
        
            _levelController.ChangeGameState(GameState.Play);
            _ballSpawner.ReverseCanThrow();
            
            _blockController.ResetDidFinish();

            _canCreate = true;
        }
    }
}
