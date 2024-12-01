using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private Image _progressSlider;
    [SerializeField] private float _sliderSpeed;
    
    [SerializeField] private float _maxProgress;
    [SerializeField] private Color _firstProgressColor;
    [SerializeField] private GameObject _firstProgressModel;
    [SerializeField] private float _firstProgress;
    
    [SerializeField] private Color _secondProgressColor;
    [SerializeField] private GameObject _secondProgressModel;
    [SerializeField] private float _secondProgress;
    
    [SerializeField] private Color _thirdProgressColor;
    [SerializeField] private float _thirdProgress;
    [SerializeField] private GameObject _thirdProgressModel;
    
    [SerializeField] private float _addProgressCount;
    [SerializeField] private float _removeProgressCount;

    private float _progress;
    private int _state;

    public float Progress => _progress;
    public int State => _state;

    private void Start()
    {
        _progress = _firstProgress;
        UpdateSlider();
        _progressSlider.transform.parent.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        _progressSlider.transform.parent.gameObject.SetActive(true);
    }

    public void Stop()
    {
        GetComponent<PlayerController>().StopGame();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            if (_progress < _maxProgress)
                _progress += _addProgressCount;
            Destroy(other.gameObject);
            UpdateSlider();
        }

        if (other.gameObject.CompareTag("Bottle"))
        {
            _progress -= _removeProgressCount;
            Destroy(other.gameObject);
            UpdateSlider();
        }

        if (other.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.Win(4);
        }
        
        if (_progress <= 0)
            Lose();
    }

    private void Lose()
    {
        GameManager.Instance.Lose();
    }
    
    private void UpdateSlider()
    {
        //StartCoroutine(UpdateSliderRoutine());

        var progress = _progress / 100f;
        _progressSlider.fillAmount = progress;

        if (_progress <= _secondProgress)
        {
            _state = 1;
            _progressSlider.color = _firstProgressColor;
            if (!_firstProgressModel.activeSelf)
                _firstProgressModel.SetActive(true);
            _secondProgressModel.SetActive(false);
            _thirdProgressModel.SetActive(false);
        }
        else if (_progress <= _thirdProgress)
        {
            _state = 2;
            _progressSlider.color = _secondProgressColor;
            _firstProgressModel.SetActive(false);
            if (!_secondProgressModel.activeSelf)
                _secondProgressModel.SetActive(true);
            _thirdProgressModel.SetActive(false);
        }
        else if (_progress > _thirdProgress)
        {
            _state = 3;
            _progressSlider.color = _thirdProgressColor;
            _firstProgressModel.SetActive(false);
            _secondProgressModel.SetActive(false);
            if (!_thirdProgressModel.activeSelf)
                _thirdProgressModel.SetActive(true);
        }
        
        if (GetComponent<PlayerController>().IsGameStarted)
            GetComponentInChildren<Animator>().SetBool("IsGameStart", true);
    }

    private IEnumerator UpdateSliderRoutine()
    {
        var targetFillAmount = _progress / 100f;

        while (!Mathf.Approximately(_progressSlider.fillAmount, targetFillAmount))
        {
            _progressSlider.fillAmount = Mathf.MoveTowards(_progressSlider.fillAmount, targetFillAmount, _sliderSpeed * Time.deltaTime);
            yield return null;
        }
    }
}