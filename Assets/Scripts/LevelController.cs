using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private GameState _gameState;
    
    public GameState GetGameState()
    {
        return _gameState;
    }

    public void ChangeGameState(GameState state)
    {
        _gameState = state;
    }
}
