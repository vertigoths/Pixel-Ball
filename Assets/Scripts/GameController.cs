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

    [SerializeField] private GameObject boostScreen;
    
    private void Awake()
    {
        boostScreen.SetActive(false);
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

        var ballReachTime = PlayerPrefs.GetFloat("BallReachTime");

        if (ballReachTime == 0)
        {
            PlayerPrefs.SetFloat("BallReachTime", 4f);
        }
        else if(ballReachTime <= 2f)
        {
            PlayerPrefs.SetFloat("BallReachTime", ballReachTime * 2f);
        }

        StartCoroutine(LoadModel(0.75f));
    }

    private IEnumerator LoadModel(float delayTime)
    {
        if (_canRetrieve)
        {
            _canRetrieve = false;
            
            yield return new WaitForSeconds(delayTime);

            _converter.CreateThreeDimensionalModel();

            _levelController.ChangeGameState(GameState.Play);
            _ballSpawner.ReverseCanThrow();
            
            _blockController.ResetDidFinish();
            
            _canRetrieve = true;
            boostScreen.SetActive(true);
        }
    }
}
