using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Progress Settings")]
    [SerializeField] private Image progressSlider;
    [SerializeField] private float sliderSpeed;
    
    [Header("Progress Levels")]
    [SerializeField] private float maxProgress;
    [SerializeField] private Color firstProgressColor;
    [SerializeField] private GameObject firstProgressModel;
    [SerializeField] private float firstProgress;
    
    [SerializeField] private Color secondProgressColor;
    [SerializeField] private GameObject secondProgressModel;
    [SerializeField] private float secondProgress;
    
    [SerializeField] private Color thirdProgressColor;
    [SerializeField] private float thirdProgress;
    [SerializeField] private GameObject thirdProgressModel;
    
    [Header("Progress Modifiers")]
    [SerializeField] private float addProgressCount;
    [SerializeField] private float removeProgressCount;

    [Header("Effects")]
    [SerializeField] private GameObject addProgressEffect;
    [SerializeField] private GameObject removeProgressEffect;
    
    private float progress;
    private int state;

    public float Progress => progress;
    public int State => state;

    private void Awake()
    {
        progressSlider.transform.parent.gameObject.SetActive(false);
    }

    private void Start()
    {
        progress = firstProgress;
        UpdateSlider();
    }

    public void StartGame()
    {
        progressSlider.transform.parent.gameObject.SetActive(true);
    }

    public void StopGame()
    {
        GetComponent<PlayerController>().StopGame();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Money"))
        {
            if (progress < maxProgress)
                progress += addProgressCount;
            Destroy(other.gameObject);
            var obj = Instantiate(addProgressEffect, transform);
            Destroy(obj, 3);
            UpdateSlider();
        }

        if (other.gameObject.CompareTag("Bottle"))
        {
            progress -= removeProgressCount;
            Destroy(other.gameObject);
            var obj = Instantiate(removeProgressEffect, transform);
            Destroy(obj, 3);
            UpdateSlider();
        }

        if (other.gameObject.CompareTag("Win"))
        {
            GameManager.Instance.Win(4);
        }
        
        if (progress <= 0)
            Lose();
    }

    private void Lose()
    {
        GameManager.Instance.Lose();
    }
    
    private void UpdateSlider()
    {
        StartCoroutine(UpdateSliderRoutine());
    }

    private IEnumerator UpdateSliderRoutine()
    {
        float targetFillAmount = progress / 100f;
        float initialFillAmount = progressSlider.fillAmount;
        float elapsedTime = 0f;
        float duration = 0.5f; 

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            progressSlider.fillAmount = Mathf.Lerp(initialFillAmount, targetFillAmount, elapsedTime / duration);
            yield return null;
        }

        progressSlider.fillAmount = targetFillAmount;

        if (progress <= secondProgress)
        {
            state = 1;
            progressSlider.color = firstProgressColor;
            if (!firstProgressModel.activeSelf)
                firstProgressModel.SetActive(true);
            secondProgressModel.SetActive(false);
            thirdProgressModel.SetActive(false);
        }
        else if (progress <= thirdProgress)
        {
            state = 2;
            progressSlider.color = secondProgressColor;
            firstProgressModel.SetActive(false);
            if (!secondProgressModel.activeSelf)
                secondProgressModel.SetActive(true);
            thirdProgressModel.SetActive(false);
        }
        else if (progress > thirdProgress)
        {
            state = 3;
            progressSlider.color = thirdProgressColor;
            firstProgressModel.SetActive(false);
            secondProgressModel.SetActive(false);
            if (!thirdProgressModel.activeSelf)
                thirdProgressModel.SetActive(true);
        }

        if (GetComponent<PlayerController>().IsGameStarted)
            GetComponentInChildren<Animator>().SetBool("IsGameStart", true);
    }
}