using TMPro;
using UnityEngine;

public class GameplayHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _scoreText;

    private int _currentScore;
    
    private void Start()
    {
        _currentScore = 0;
        UpdateScore();
    }
    
    private void OnEnable()
    {
        EventManager.OnFireballDestroyed += OnFireballDestroyed;
    }

    private void OnDisable()
    {
        EventManager.OnFireballDestroyed -= OnFireballDestroyed;
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
