using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private Text _moveText;

    [Header("Timer Settings")]
    [SerializeField] private float _showMoveTutorialTime = 10f; //動きのチュートリアル表示時間
    private float _timer;

    private bool _canUseTimer; //時間計測中か判定処理
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //タイマー初期化
        _timer = 0;

        ShowMoveTutorial();
    }

    void Update()
    {
        if (!_canUseTimer) return;

        _timer += Time.deltaTime;

        if (_timer >= _showMoveTutorialTime)
        { 
            _moveText.gameObject.SetActive(false);
            _canUseTimer = false;
        }
    }

    /// <summary>
    /// 動きのチュートリアル表示処理
    /// </summary>
    private void ShowMoveTutorial()
    {
        _moveText.gameObject.SetActive(true);

        _canUseTimer = true;
    }
}
