using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class BlockController : MonoBehaviour
{
    private GameObject[,] _map;
    private int _blockCount;

    private bool _didFinish = false;
    
    public void RemoveFromMap(int posX, int posY)
    {
        _map[posY, posX] = null;
        _blockCount -= 1;

        if (CheckIfAllBlocksRemoved())
        {
            FindObjectOfType<LevelController>().ChangeGameState(GameState.End);
        }
        
        if (CheckIfThereIsLessBlocks())
        {
            FindObjectOfType<BallSpawner>().ReverseCanThrow();
            RemoveRemainingBlocks();

            StartCoroutine(FindObjectOfType<GameController>().StartGame(3f));
            _blockCount = 0;
            _didFinish = true;
        }
    }
    
    public bool CheckIfAllBlocksRemoved()
    {
        return _blockCount == 0;
    }

    private bool CheckIfThereIsLessBlocks()
    {
        return _blockCount <= 20;
    }

    public bool GetDidFinish()
    {
        return _didFinish;
    }

    public void ResetDidFinish()
    {
        _didFinish = false;
    }

    public void SetMapReference(GameObject[,] map)
    {
        _map = map;

        StoreBlocksCount(_map);
        FindObjectOfType<UIController>().SetTotalBlockCount(_blockCount);
    }

    private void StoreBlocksCount(GameObject[,] map)
    {
        for (var i = 0; i < map.GetLength(0); i++)
        {
            for (var j = 0; j < map.GetLength(1); j++)
            {
                if (map[i, j])
                {
                    _blockCount++;
                }
            }
        }
    }

    public GameObject GetBlock()
    {
        var availableBlocks = new List<GameObject>();
        
        for (var i = 0; i < _map.GetLength(0); i++)
        {
            for (var j = 0; j < _map.GetLength(1); j++)
            {
                if ((i == 0 && _map[i, j]) || (_map[i, j] && !_map[i - 1, j]))
                {
                    availableBlocks.Add(_map[i, j]);
                }
            }
        }

        var randomBlockIndex = Random.Range(0, availableBlocks.Count);

        return availableBlocks[randomBlockIndex];
    }

    private void RemoveRemainingBlocks()
    {
        for (var i = 0; i < _map.GetLength(0); i++)
        {
            for (var j = 0; j < _map.GetLength(1); j++)
            {
                if (_map[i, j])
                {
                    _map[i,j].GetComponent<PixelBlock>().OnBallTouch(null);
                    _map[i, j] = null;
                }
            }
        }
    }

    public bool IsExistInMap(int posY, int posX)
    {
        return _map[posY, posX];
    }
}
