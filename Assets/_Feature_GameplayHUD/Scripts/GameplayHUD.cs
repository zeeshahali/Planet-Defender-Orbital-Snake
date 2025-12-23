using System.Globalization;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _healthText;
    
    [SerializeField] private RectTransform _gameOverPanel;

    [SerializeField] private Image _healthEffectImage;
    [SerializeField] private Image _healthImage;
    
    [SerializeField] private Button ResetButton;
    [SerializeField] private Button RestartButton;

    private int _currentScore;
    
    private void Start()
    {
        _currentScore = 0;
        UpdateScore();
    }
    
    private void OnEnable()
    {
        EventManager.OnFireballDestroyed += OnFireballDestroyed;
        EventManager.OnGameOver += OnGameOver;
        EventManager.OnPlanetHealthUpdate += OnPlanetHealthUpdate;
        ResetButton.onClick.AddListener(OnResetButtonClicked);
        RestartButton.onClick.AddListener(OnResetButtonClicked);
    }

    private void OnDisable()
    {
        EventManager.OnFireballDestroyed -= OnFireballDestroyed;
        EventManager.OnGameOver -= OnGameOver;
        EventManager.OnPlanetHealthUpdate -= OnPlanetHealthUpdate;
        ResetButton.onClick.RemoveListener(OnResetButtonClicked);
        RestartButton.onClick.RemoveListener(OnResetButtonClicked);
    }

    private void OnResetButtonClicked()
    {
        EventManager.TriggerGameOver();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void OnGameOver()
    {
        ResetButton.gameObject.SetActive(false);
        _gameOverPanel.localScale = Vector3.zero;
        _gameOverPanel.gameObject.SetActive(true);
        _gameOverPanel.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutBack);
    }

    private void OnFireballDestroyed()
    {
        _currentScore++;
        UpdateScore();
    }

    private Sequence _healthUpdateSequence;
    
    private void OnPlanetHealthUpdate(float health)
    {
        var currentValue = _healthImage.fillAmount * 100;

        var fillAmount = health / 100;
        
        _healthUpdateSequence = DOTween.Sequence();
        
        _healthUpdateSequence.Append(_healthEffectImage.DOFade(1f, 0.2f))
            .Append(_healthImage.DOFillAmount(fillAmount, 0.2f))
            .Append(_healthEffectImage.DOFillAmount(fillAmount, 0.2f))
            .Join(_healthEffectImage.DOFade(0, 0.2f))
            .Join(DOTween.To(()=> currentValue, x =>
            {
                currentValue = x;
                _healthText.text = currentValue.ToString("F2");
            }, health, 0.2f));
        
        _healthUpdateSequence.OnKill(()=> UpdatePlanetHealth(health));
    }

    private void UpdatePlanetHealth(float health)
    {
        _healthImage.fillAmount = health/100;
        _healthEffectImage.fillAmount = health/100;
        _healthText.text = health.ToString(CultureInfo.InvariantCulture);
    }

    private void UpdateScore()
    {
        _scoreText.text = $"Score: {_currentScore}";
    }
}
