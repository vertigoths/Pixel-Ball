using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private int _currentLevel;
    private GameState _gameState;

    private void Start()
    {
        _currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        _gameState = GameState.Menu;
    }

    public GameState GetGameState()
    {
        return _gameState;
    }

    public int GetCurrentLevel()
    {
        return _currentLevel;
    }

    public void ChangeGameState(GameState state)
    {
        _gameState = state;
    }
}
