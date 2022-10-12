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
    private UIController _uiController;

    private bool _canBuy;

    [SerializeField] private TMP_Text moneyText;

    private void Awake()
    {
        OnCreate();
    }

    private void OnCreate()
    {
        _currentLevel = PlayerPrefs.GetInt(upgradeType);
        _priceText = transform.GetChild(3).GetComponent<TMP_Text>();
        _image = GetComponent<Image>();
        _image.color = Color.gray;
        _ballSpawner = FindObjectOfType<BallSpawner>();
        _uiController = FindObjectOfType<UIController>();

        _priceData = GetRelatedData();

        if (_currentLevel == _priceData.Length)
        {
            _priceText.text = "MAX";
        }
        else
        {
            _priceText.text = "$" + _priceData[_currentLevel];
        }
    }
    
    private void Increase()
    {
        if (_canBuy && _currentLevel < _priceData.Length)
        {
            var price = _priceData[_currentLevel];
            var nextMoney = PlayerPrefs.GetInt("CurrentMoney") - price;
            PlayerPrefs.SetInt("CurrentMoney", nextMoney);
        
            if (upgradeType.Equals("Ball"))
            {
                _ballSpawner.IncreaseCountOfBall();
            }
            else if (upgradeType.Equals("Time"))
            {
                _ballSpawner.DecreaseBallReachTime();
            }
            else if(upgradeType.Equals("Money"))
            {
                _uiController.IncreaseMoneyGetAmount();
            }

            if (_currentLevel == _priceData.Length)
            {
                _priceText.text = "MAX";
                _image.color = Color.red;
            }
            else
            {
                _uiController.SetCurrentMoney(nextMoney);
                
                moneyText.text = "$" + nextMoney;
                _image.color = Color.grey;
                _canBuy = false;
                
                PlayerPrefs.SetInt(upgradeType, ++_currentLevel);
                
                _priceText.text = "$" + _priceData[_currentLevel];
                
                _ballSpawner.SetProgressText();
            }
        }
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

    public void OnMoneyCollect(int currentMoney)
    {
        if (_currentLevel != _priceData.Length && currentMoney >= _priceData[_currentLevel])
        {
            _image.color = Color.green;
            _canBuy = true;
        }
    }

    public void CheckIfCanBuy(int nextMoney)
    {
        if (_currentLevel != _priceData.Length && nextMoney < _priceData[_currentLevel])
        {
            _image.color = Color.grey;
            _canBuy = false;
        }
    }
}