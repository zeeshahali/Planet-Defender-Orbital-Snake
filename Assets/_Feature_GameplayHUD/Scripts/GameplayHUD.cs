using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameplayHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Button ResetButton;

    private int _currentScore;
    
    private void Start()
    {
        _currentScore = 0;
        UpdateScore();
    }
    
    private void OnEnable()
    {
        EventManager.OnFireballDestroyed += OnFireballDestroyed;
        ResetButton.onClick.AddListener(OnResetButtonClicked);
    }

    private void OnDisable()
    {
        EventManager.OnFireballDestroyed -= OnFireballDestroyed;
        ResetButton.onClick.RemoveListener(OnResetButtonClicked);
    }

    private void OnResetButtonClicked()
    {
        EventManager.TriggerGameReset();
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    private void OnFireballDestroyed()
    {
        _currentScore++;
        UpdateScore();
    }

    private void UpdateScore()
    {
        _scoreText.text = $"Score: {_currentScore}";
    }
}
