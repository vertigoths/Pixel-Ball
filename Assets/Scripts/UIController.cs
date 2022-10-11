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

    [SerializeField] private Upgrade[] upgrades;

    private bool _letBoost = true;

    private BallSpawner _ballSpawner;
    
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
        ProgressBar.Instance.OnBlockCollect();
        CreateGroundMoney(pos);
    }

    private void CreateGroundMoney(Vector3 pos)
    {
        if (_groundMoneys.Count > 0)
        {
            var groundMoney = _groundMoneys.First.Value;
            _groundMoneys.RemoveFirst();
            groundMoney.SetActive(true);
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
        StartCoroutine(ControlScreenTouch());
    }

    private IEnumerator ControlScreenTouch()
    {
        if (_letBoost)
        {
            _letBoost = false;
            
            Vibrations.Haptic (HapticTypes.LightImpact);
            
            _ballSpawner.BoostBallReachTime();
            _ballSpawner.SetProgressText();

            yield return new WaitForSeconds(0.25f);
            
            _ballSpawner.TakeBackBallReachTimeBoost();
            _ballSpawner.SetProgressText();

            _letBoost = true;
        }
        
    }
}
