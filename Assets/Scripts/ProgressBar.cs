using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar Instance;
    private Slider _progressBar;
    private TMP_Text _currentProgressText;
    
    private float _totalBlockCount;
    private float _currentBlockCount;

    private int _currentIterationLevel;

    private int _ballCount;
    private float _ballReachTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    public void SetTotalBlockCount(int currentBlockCount)
    {
        _currentIterationLevel = PlayerPrefs.GetInt("CurrentIterationLevel");
        _progressBar = FindObjectOfType<Slider>();
        
        _currentProgressText = _progressBar.transform.parent.GetChild(0).GetComponent<TMP_Text>();

        _ballCount = PlayerPrefs.GetInt("BallCount");
        _ballReachTime = PlayerPrefs.GetFloat("BallReachTime");

        _progressBar.value = _currentIterationLevel * 0.25f;
        
        _totalBlockCount = currentBlockCount * 4;
    }

    public void OnBlockCollect()
    {
        _progressBar.value += 1 / _totalBlockCount;
    }

    public void SetProgressText(int ballCount, float ballReachTime)
    {
        _currentProgressText.text = ballCount + " Balls/" + ballReachTime + " sec";
    }
}
