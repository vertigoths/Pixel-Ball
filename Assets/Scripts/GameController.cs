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
    
    private bool _canRetrieve = true;
    
    private void Awake()
    {
        _converter = FindObjectOfType<Converter>();
        _levelController = FindObjectOfType<LevelController>();
        _ballSpawner = FindObjectOfType<BallSpawner>();
        _blockController = FindObjectOfType<BlockController>();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("BallCount") == 0)
        {
            PlayerPrefs.SetInt("BallCount", 1);
        }

        if (PlayerPrefs.GetFloat("BallReachTime") == 0)
        {
            PlayerPrefs.SetFloat("BallReachTime", 4f);
        }

        StartCoroutine(LoadModel(0.75f));
    }

    private IEnumerator LoadModel(float delayTime)
    {
        if (PlayerPrefs.GetInt("CurrentIterationLevel") == 4)
        {
            PlayerPrefs.SetInt("CurrentIterationLevel", 0);
        }
        
        if (_canRetrieve)
        {
            _canRetrieve = false;
            
            yield return new WaitForSeconds(delayTime);
            
            _converter.CreateThreeDimensionalModel();
            PlayerPrefs.SetInt("CurrentIterationLevel", PlayerPrefs.GetInt("CurrentIterationLevel") + 1);

            _levelController.ChangeGameState(GameState.Play);
            _ballSpawner.ReverseCanThrow();
            
            _blockController.ResetDidFinish();
            
            _canRetrieve = true;
        }
    }
}
