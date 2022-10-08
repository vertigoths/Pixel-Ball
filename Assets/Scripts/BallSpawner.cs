using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSpawner : MonoBehaviour
{
    private BlockController _blockController;
    private LinkedList<GameObject> _createdBalls;

    [SerializeField] private GameObject ballPrefab;
    private readonly Vector3 _ballSpawnPosition = new Vector3(0, -0.5f, -0.75f);

    private bool[] _canSpawnBall;
    private int _countOfSpawnedBalls;

    private void Awake()
    {
        _blockController = FindObjectOfType<BlockController>();
        _createdBalls = new LinkedList<GameObject>();

        _countOfSpawnedBalls = 2;
        _canSpawnBall = new bool[_countOfSpawnedBalls];

        for (var i = 0; i < _countOfSpawnedBalls; i++)
        {
            _canSpawnBall[i] = true;
        }
    }

    private void Start()
    {
        for (var i = 0; i < 20; i++)
        {
            var spawnedBall = Instantiate(ballPrefab);
            spawnedBall.GetComponent<Ball>().OnCreate(_blockController);
            spawnedBall.SetActive(false);
            _createdBalls.AddLast(spawnedBall);
        }
    }

    private void Update()
    {
        StartCoroutine(CallBallToAction());
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
            
            yield return new WaitForSeconds(0.25f);
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
