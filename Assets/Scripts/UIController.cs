using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Voodoo.Utils;
using Random = UnityEngine.Random;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private TMP_Text currentMoneyText;
    [SerializeField] private GameObject groundMoneyPrefab;
    [SerializeField] private Canvas canvas;

    private LinkedList<GameObject> _groundMoneys;

    private Camera _camera;

    private bool _letBoost = true;

    private BallSpawner _ballSpawner;
    private int _currentMoney;
    private int _moneyGetAmount;

    [SerializeField] private Upgrade[] _upgrades;

    private void Awake()
    {
        _camera = FindObjectOfType<Camera>();
        _ballSpawner = FindObjectOfType<BallSpawner>();
    }

    private void Start()
    {
        var currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevelText.text = (currentLevel + 1).ToString();
        nextLevelText.text = (currentLevel + 2).ToString();

        _currentMoney = PlayerPrefs.GetInt("CurrentMoney");
        currentMoneyText.text = "$" + _currentMoney;

        _moneyGetAmount = PlayerPrefs.GetInt("MoneyGetAmount");
        if (_moneyGetAmount == 0)
        {
            _moneyGetAmount = 1;
        }

        _groundMoneys = new LinkedList<GameObject>();

        for (var i = 0; i < 8; i++)
        {
            var groundMoney = Instantiate(groundMoneyPrefab, canvas.transform, true);
            groundMoney.SetActive(false);
            _groundMoneys.AddLast(groundMoney);
        }
    }

    public void OnBlockCollect(Vector3 pos)
    {
        _currentMoney += _moneyGetAmount;
        PlayerPrefs.SetInt("CurrentMoney", _currentMoney);
        currentMoneyText.text = "$" + _currentMoney;

        ProgressBar.Instance.OnBlockCollect();
        CreateGroundMoney(pos);

        for (var i = 0; i < _upgrades.Length; i++)
        {
            _upgrades[i].OnMoneyCollect(_currentMoney);
        }
    }

    public void SetCurrentMoney(int nextMoney)
    {
        _currentMoney = nextMoney;

        for (var i = 0; i < _upgrades.Length; i++)
        {
            _upgrades[i].CheckIfCanBuy(nextMoney);
        }
    }

    private void CreateGroundMoney(Vector3 pos)
    {
        if (_groundMoneys.Count > 0)
        {
            var groundMoney = _groundMoneys.First.Value;
            _groundMoneys.RemoveFirst();
            groundMoney.SetActive(true);
            groundMoney.GetComponent<TMP_Text>().text = "$" + _moneyGetAmount;
            groundMoney.transform.position = _camera.WorldToScreenPoint(pos);

            groundMoney.transform.DOLocalMoveY(0f, Random.Range(2f, 2.5f));
            StartCoroutine(ReturnMoneyTextBack(groundMoney));
        }
    }

    private IEnumerator ReturnMoneyTextBack(GameObject groundMoney)
    {
        yield return new WaitForSeconds(0.25f);
        groundMoney.SetActive(false);
        groundMoney.transform.DOKill();
        _groundMoneys.AddLast(groundMoney);
    }

    public void OnScreenTouch()
    {
        Vibrations.Haptic(HapticTypes.LightImpact);
        StartCoroutine(ControlScreenTouch());
    }

    private IEnumerator ControlScreenTouch()
    {
        if (_letBoost)
        {
            _letBoost = false;

            _ballSpawner.BoostBallReachTime();
            _ballSpawner.SetProgressText();

            yield return new WaitForSeconds(1f);

            _ballSpawner.TakeBackBallReachTimeBoost();

            _ballSpawner.SetProgressText();

            _letBoost = true;
        }
    }

    public void IncreaseMoneyGetAmount()
    {
        PlayerPrefs.SetInt("MoneyGetAmount", ++_moneyGetAmount);
    }
}