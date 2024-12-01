using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Player player;

    [Header("UI Elements")]
    [SerializeField] private Image fadeInEffect;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private GameObject tutorialUI;
    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject loseUI;
    
    public static GameManager Instance { get; private set; }
    
    private bool isGameStart;
    private static int coins;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        StartCoroutine(FadeOnRoutine());
        coinsText.text = coins.ToString();
    }

    private void StartGame()
    {
        playerController.StartGame();
        player.StartGame();
        tutorialUI.SetActive(false);
    }

    private void Update()
    {
        if (!isGameStart && Input.GetMouseButtonDown(0))
        {
            isGameStart = true;
            StartGame();
        }
    }
    
    public void Win(int state)
    {
        winUI.SetActive(true);
        playerController.StopGame();
        coins += Mathf.RoundToInt(player.Progress * state);
        coinsText.text = coins.ToString();
    }
    
    public void Lose()
    {
        loseUI.SetActive(true);
        playerController.StopGame();
    }

    public void Restart()
    {
        StartCoroutine(FadeInRoutine(0));
    }

    private IEnumerator FadeInRoutine(int sceneId)
    {
        while (fadeInEffect.color.a < 1)
        {
            fadeInEffect.color = new Color(0, 0, 0, fadeInEffect.color.a + 0.05f);
            yield return null;
        }
        
        SceneManager.LoadScene(sceneId);
    }
    
    private IEnumerator FadeOnRoutine()
    {
        while (fadeInEffect.color.a > 0)
        {
            fadeInEffect.color = new Color(0, 0, 0, fadeInEffect.color.a - 0.05f);
            yield return null;
        }
    }
}