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
    private GameObject[] _spawnedBalls;
    
    private int _countOfSpawnedBalls;
    
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

        _countOfSpawnedBalls = 1;

        _canSpawnBall = new bool[_countOfSpawnedBalls];
        _spawnedBalls = new GameObject[_countOfSpawnedBalls];

        for (var i = 0; i < _countOfSpawnedBalls; i++)
        {
            _canSpawnBall[i] = true;
        }
        
        _uiController.SetProgressText(_countOfSpawnedBalls, 0f);
        
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
            CallBallToAction();
        }
    }

    public void ReverseCanThrow()
    {
        _canThrow = !_canThrow;
    }

    private void CallBallToAction()
    {
        for (var i = 0; i < _countOfSpawnedBalls; i++)
        {
            if (_canSpawnBall[i] && !_blockController.CheckIfAllBlocksRemoved())
            {
                _canSpawnBall[i] = false;

                var targetBlock = _blockController.GetBlock();
            
                if (_createdBalls.Count != 0)
                {
                    TryToSendBall(targetBlock, i);
                }
            }
        }
    }

    private void TryToSendBall(GameObject targetBlock, int index)
    {
        var calledBall = _createdBalls.Last.Value;
        _createdBalls.RemoveLast();
        
        if (targetBlock)
        {
            calledBall.SetActive(true);
            //calledBall.transform.position = _ballSpawnPosition + new Vector3(Random.Range(-1f, 1f), 0f, 0f);

            _spawnedBalls[index] = calledBall;
            calledBall.GetComponent<Ball>().OnCall(targetBlock.transform.position);
        }

        else
        {
            _createdBalls.AddLast(calledBall);
        }
    }

    public void AddBallBack(GameObject returnedBall)
    {
        for (var i = 0; i < _spawnedBalls.Length; i++)
        {
            if (_spawnedBalls[i] == returnedBall)
            {
                _canSpawnBall[i] = true;
            }
        }
        _createdBalls.AddLast(returnedBall);
    }
}
