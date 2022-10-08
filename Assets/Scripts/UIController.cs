using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private TMP_Text currentLevelText;
    [SerializeField] private TMP_Text nextLevelText;
    [SerializeField] private Slider progressBar;

    private float _totalBlockCount;
    private float _currentBlockCount;

    private void Start()
    {
        var currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        currentLevelText.text = (currentLevel + 1).ToString();
        nextLevelText.text = (currentLevel + 2).ToString();
    }

    public void OnBlockCollect()
    {
        _currentBlockCount += 1;
        progressBar.value = _currentBlockCount / _totalBlockCount;
    }

    public void SetTotalBlockCount(int totalBlockCount)
    {
        _totalBlockCount = totalBlockCount;
    }
}
