using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    private BlockController _blockController;
    private LevelController _levelController;
    private UIController _uiController;
    private LinkedList<GameObject> _createdBalls;

    [SerializeField] private GameObject ballPrefab;
    private readonly Vector3 _ballSpawnPosition = new Vector3(0, -0.5f, -0.75f);

    private bool[] _canSpawnBall;
    private int _countOfSpawnedBalls;
    private float _delayBetweenThrows;

    private bool _canThrow;

    private void Awake()
    {
        _blockController = FindObjectOfType<BlockController>();
        _levelController = FindObjectOfType<LevelController>();
        _uiController = FindObjectOfType<UIController>();
    }

    private void Start()
    {
        var parent = new GameObject();
        _createdBalls = new LinkedList<GameObject>();

        _countOfSpawnedBalls = 4;
        _delayBetweenThrows = 1f;
        
        _canSpawnBall = new bool[_countOfSpawnedBalls];

        for (var i = 0; i < _countOfSpawnedBalls; i++)
        {
            _canSpawnBall[i] = true;
        }
        
        _uiController.SetProgressText(_countOfSpawnedBalls, _delayBetweenThrows);
        
        for (var i = 0; i < 20; i++)
        {
            var spawnedBall = Instantiate(ballPrefab, parent.transform, true);
            spawnedBall.GetComponent<Ball>().OnCreate(_blockController);
            spawnedBall.SetActive(false);
            _createdBalls.AddLast(spawnedBall);
        }
    }

    private void Update()
    {
        if (_levelController.GetGameState() == GameState.Play && _canThrow)
        {
            StartCoroutine(CallBallToAction());
        }
    }

    public void ReverseCanThrow()
    {
        _canThrow = !_canThrow;
    }

    private IEnumerator CallBallToAction()
    {
        for (var i = 0; i < _countOfSpawnedBalls; i++)
        {
            if (_canSpawnBall[i] && !_blockController.CheckIfAllBlocksRemoved())
            {
                _canSpawnBall[i] = false;

                var targetBlock = _blockController.GetBlock();
            
                if (_createdBalls.Count != 0)
                {
                    TryToSendBall(targetBlock);
                }

                yield return new WaitForSeconds(1.5f);

                _canSpawnBall[i] = true;
            }
            
            yield return new WaitForSeconds(Random.Range(0.2f, 0.3f));
        }
    }

    private void TryToSendBall(GameObject targetBlock)
    {
        var calledBall = _createdBalls.First.Value;
        _createdBalls.RemoveFirst();
        
        if (targetBlock)
        {
            calledBall.SetActive(true);
            calledBall.transform.position = _ballSpawnPosition + new Vector3(Random.Range(-1f, 1f), 0f, 0f);

            calledBall.GetComponent<Ball>().OnCall(targetBlock.transform.position);
        }

        else
        {
            _createdBalls.AddLast(calledBall);
        }
    }

    public void AddBallBack(GameObject returnedBall)
    {
        returnedBall.SetActive(false);
        _createdBalls.AddLast(returnedBall);
    }
}
