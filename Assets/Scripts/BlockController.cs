using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BlockController : MonoBehaviour
{
    private GameObject[,] _map;
    private int _blockCount;

    private bool _didFinish;
    private int _currentIterationCount;

    [SerializeField] private GameObject confettiPrefab;
    [SerializeField] private Image _image;

    private LevelController _levelController;
    [SerializeField] private GameObject boostScreen;

    private void Awake()
    {
        _levelController = FindObjectOfType<LevelController>();
    }

    public void RemoveFromMap(int posX, int posY)
    {
        _map[posY, posX] = null;
        _blockCount -= 1;

        if (CheckIfThereIsLessBlocks())
        {
            FindObjectOfType<BallSpawner>().ReverseCanThrow();
            RemoveRemainingBlocks();
            
            _blockCount = 0;
            _didFinish = true;
            
            StartCoroutine(OnGameEnd());
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
        ProgressBar.Instance.SetTotalBlockCount(_blockCount);
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
                    _map[i, j].GetComponent<PixelBlock>().OnBallTouch(null);
                    _map[i, j] = null;
                }
            }
        }
    }

    public bool IsExistInMap(int posY, int posX)
    {
        return _map[posY, posX];
    }

    private IEnumerator OnGameEnd()
    {
        boostScreen.SetActive(false);
        _levelController.ChangeGameState(GameState.End);
        Instantiate(confettiPrefab);
        PlayerPrefs.SetInt("CurrentIterationLevel", PlayerPrefs.GetInt("CurrentIterationLevel") + 1);
        
        if (PlayerPrefs.GetInt("CurrentIterationLevel") == 4)
        {
            PlayerPrefs.SetInt("CurrentIterationLevel", 0);
            var currentLevel = PlayerPrefs.GetInt("CurrentLevel") + 1;

            PlayerPrefs.DeleteAll();
            
            PlayerPrefs.SetInt("CurrentLevel", currentLevel);
            
            _image.color = Color.green;
        }

        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("Game");
    }
}
