using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private string upgradeType;
    private int _currentLevel;
    private TMP_Text _priceText;
    private Image _image;

    private int[] _priceData;

    private BallSpawner _ballSpawner;

    private void Awake()
    {
        OnCreate();
    }

    private void OnCreate()
    {
        _currentLevel = PlayerPrefs.GetInt(upgradeType);
        _priceText = transform.GetChild(3).GetComponent<TMP_Text>();
        _image = GetComponent<Image>();
        _ballSpawner = FindObjectOfType<BallSpawner>();

        _priceData = GetRelatedData();

        _priceText.text = "$" + _priceData[_currentLevel];
    }
    
    private void Increase()
    {
        PlayerPrefs.SetInt(upgradeType, ++_currentLevel);
        
        if (upgradeType.Equals("Ball"))
        {
            _ballSpawner.IncreaseCountOfBall();
        }
        else if (upgradeType.Equals("Time"))
        {
            _ballSpawner.TakeBackBallReachTimeBoost();
            _ballSpawner.DecreaseBallReachTime();
        }
        else
        {
            
        }
        
        _ballSpawner.SetProgressText();
    }

    public void OnClick()
    {
        Increase();
    }

    private int[] GetRelatedData()
    {
        return upgradeType switch
        {
            "Ball" => GameData.BallPrices,
            "Time" => GameData.TimePrices,
            _ => GameData.IncomePrices
        };
    }

    public void OnMoneyIncrease(int currentMoney)
    {
        
    }
}