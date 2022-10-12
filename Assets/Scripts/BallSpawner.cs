using System.Collections.Generic;
using Enums;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    private BlockController _blockController;
    private LevelController _levelController;
    private LinkedList<GameObject> _createdBalls;

    [SerializeField] private GameObject ballPrefab;

    private bool[] _canSpawnBall;
    private GameObject[] _spawnedBalls;
    
    private int _countOfSpawnedBalls;
    
    private bool _canThrow;

    private float _ballReachTime;

    private void Awake()
    {
        _blockController = FindObjectOfType<BlockController>();
        _levelController = FindObjectOfType<LevelController>();
    }

    private void Start()
    {
        var parent = new GameObject();
        _createdBalls = new LinkedList<GameObject>();
        
        _countOfSpawnedBalls = PlayerPrefs.GetInt("BallCount");
        if (_countOfSpawnedBalls == 0)
        {
            _countOfSpawnedBalls = 1;
        }

        _ballReachTime = PlayerPrefs.GetFloat("BallReachTime");
        if (_ballReachTime == 0)
        {
            _ballReachTime = 4f;
        }
        else if(_ballReachTime <= 2f)
        {
            PlayerPrefs.SetFloat("BallReachTime", _ballReachTime * 2f);
        }

        _canSpawnBall = new bool[_countOfSpawnedBalls];
        _spawnedBalls = new GameObject[_countOfSpawnedBalls];

        for (var i = 0; i < _countOfSpawnedBalls; i++)
        {
            _canSpawnBall[i] = true;
        }

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

            _spawnedBalls[index] = calledBall;
            calledBall.GetComponent<Ball>().OnCall(targetBlock.transform.position, _ballReachTime);
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

    public void IncreaseCountOfBall()
    {
        _countOfSpawnedBalls++;
        _canSpawnBall = new bool[_countOfSpawnedBalls];
        _spawnedBalls = new GameObject[_countOfSpawnedBalls];

        for (var i = 0; i < _countOfSpawnedBalls; i++)
        {
            _canSpawnBall[i] = true;
        }

        PlayerPrefs.SetInt("BallCount", _countOfSpawnedBalls);
    }

    public void BoostBallReachTime()
    {
        _ballReachTime *= 0.5f;
    }

    public void TakeBackBallReachTimeBoost()
    {
        _ballReachTime *= 2f;
    }

    public void DecreaseBallReachTime()
    {
        if (_ballReachTime > 2f)
        {
            _ballReachTime -= 0.2f;
        }
        else
        {
            _ballReachTime -= 0.1f;
        }
        
        PlayerPrefs.SetFloat("BallReachTime", _ballReachTime);
    }

    public void SetProgressText()
    {
        ProgressBar.Instance.SetProgressText(_countOfSpawnedBalls, _ballReachTime);
    }
}
