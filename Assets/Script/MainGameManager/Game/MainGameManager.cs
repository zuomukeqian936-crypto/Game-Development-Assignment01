using System;
using UnityEngine;

public class MainGameManager : MonoBehaviour
{
    public enum MainGameState //mainGameのモード
    {
        WaveEarly,
        WaveMid,
        WaveLast,
        WaveBoss
    }
    private MainGameState nowMode = MainGameState.WaveEarly;
    private MainGameState nextMode = MainGameState.WaveEarly;

    [Header("Boss Spawn Time")]
    [SerializeField] private float _bossSpawnTime = 10f;
    private float _currentTimer;
    public float _CurrentTimer => _currentTimer;

    private int _modeChangeCount;//モードチェンジの数
    private int _modeDecreaseAmount = 1; //モードの減らす数
    private float _waveTime;//一つのウェーブ時間

    private bool _isGameEnd;

    public event Action OnUIInitialized;
    public event Action<float> OnTimerChanged;

    private void Start()
    {
        _modeChangeCount = Enum.GetValues(typeof(MainGameState)).Length - _modeDecreaseAmount;

        _waveTime = _bossSpawnTime / _modeChangeCount;

        Debug.Log($"モード数：{_modeChangeCount}インターバル時間：{_waveTime} MainGameManagerで使用");

        OnUIInitialized?.Invoke();
    }

    void Update()
    {
        if (_isGameEnd) return;

        UpdateModeByTimer();

        switch (nowMode)
        {
            case MainGameState.WaveEarly:
                UpdateEarlyMode();
                break;

            case MainGameState.WaveMid:
                UpdateMidMode();
                break;

            //case MainGameState.WaveSubBuss:
            //    UpdateSubBossMode();
            //    break;

            case MainGameState.WaveLast:
                UpdateLastMode();
                break;

            case MainGameState.WaveBoss:
                UpdateBossMode();
                break;
        }
    }

    /// <summary>
    /// 時間に応じてゲームモードの変更処理
    /// </summary>
    private void UpdateModeByTimer()
    {
        _currentTimer += Time.deltaTime;

        OnTimerChanged?.Invoke(_currentTimer);

        if (nextMode == nowMode) return;

        if (_currentTimer < _waveTime)
        {
            nextMode = MainGameState.WaveEarly;
        }
        else if(_currentTimer < _waveTime * 2)
        {
            nextMode = MainGameState.WaveMid;
        }
        else if(_bossSpawnTime < _currentTimer)
        {
            nextMode = MainGameState.WaveLast;
        }

        nowMode = nextMode;
    }

    private void UpdateEarlyMode()
    {

    }

    private void UpdateMidMode()
    {

    }

    private void UpdateLastMode()
    {

    }

    private void UpdateBossMode()
    {

    }
}
