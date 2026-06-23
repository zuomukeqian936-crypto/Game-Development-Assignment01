using UnityEngine;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private MainGameManager _mainGameManager;

    [Header("UI Settings")]
    [SerializeField] private Text _timerText;

    private void OnEnable()
    {
        _mainGameManager.OnTimerChanged += UpdateTimerUI;
        _mainGameManager.OnUIInitialized += HandleUIInitialized;
    }
    private void HandleUIInitialized()
    {
        _timerText.gameObject.SetActive(true);
    }

    private void UpdateTimerUI(float timer)
    {
        _timerText.text = timer.ToString("N0");
    }

    private void OnDisable()
    {
        _mainGameManager.OnTimerChanged -= UpdateTimerUI;
        _mainGameManager.OnUIInitialized -= HandleUIInitialized;
    }
}
