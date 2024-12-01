using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Player _player;
    [SerializeField] private Image _fadeInEffect;
    [SerializeField] private TMP_Text _coinsText;
    [SerializeField] private GameObject _tutUIGameObject;
    [SerializeField] private GameObject _winUIGameObject;
    [SerializeField] private GameObject _loseUIGameObject;
    
    public static GameManager Instance { get; private set; }
    
    private bool _isGameStart;
    private static int _coins;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        StartCoroutine(FadeOnRoutine());
        _coinsText.text = _coins.ToString();
    }

    private void StartGame()
    {
        _playerController.StartGame();
        _player.StartGame();
        _tutUIGameObject.SetActive(false);
    }

    private void Update()
    {
        if (!_isGameStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _isGameStart = true;
                StartGame();
            }
        }
    }
    
    public void Win(int state)
    {
        _winUIGameObject.SetActive(true);
        _playerController.StopGame();
        _coins += Mathf.RoundToInt(_player.Progress);
        _coinsText.text = _coins.ToString();
    }
    
    public void Lose()
    {
        _loseUIGameObject.SetActive(true);
        _playerController.StopGame();
    }

    public void Restart()
    {
        StartCoroutine(FadeInRoutine(0));
    }

    private IEnumerator FadeInRoutine(int sceneId)
    {
        while (_fadeInEffect.color.a < 1)
        {
            _fadeInEffect.color = new Color(0, 0, 0, _fadeInEffect.color.a + 0.05f);
            yield return null;
        }
        
        SceneManager.LoadScene(sceneId);
    }
    
    private IEnumerator FadeOnRoutine()
    {
        while (_fadeInEffect.color.a > 0)
        {
            _fadeInEffect.color = new Color(0, 0, 0, _fadeInEffect.color.a - 0.05f);
            yield return null;
        }
    }
}