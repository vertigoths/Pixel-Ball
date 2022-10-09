using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private TMP_Text currentMoneyText;
    [SerializeField] private TMP_Text currentProgressText;
    [SerializeField] private GameObject groundMoneyPrefab;
    [SerializeField] private Canvas canvas;
    
    [SerializeField] private Slider progressBar;
    
    private LinkedList<GameObject> _groundMoneys;

    private Camera _camera;

    private float _totalBlockCount;
    private float _currentBlockCount;

    private BlockController _blockController;

    private void Awake()
    {
        _blockController = FindObjectOfType<BlockController>();
        _camera = FindObjectOfType<Camera>();
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
        _currentBlockCount += 1;
        progressBar.value = _currentBlockCount / _totalBlockCount;
        CreateGroundMoney(pos);
    }

    public void SetTotalBlockCount(int totalBlockCount)
    {
        _currentBlockCount = 0;
        _totalBlockCount = totalBlockCount;
        progressBar.value = _currentBlockCount / _totalBlockCount;
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

    public void SetProgressText(int ballCount, float delayCount)
    {
        currentProgressText.text = ballCount + " Balls/" + delayCount + " sec";
    }
}
