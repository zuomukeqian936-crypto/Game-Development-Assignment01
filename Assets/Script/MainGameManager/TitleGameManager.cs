using DG.Tweening;
using Spine.Unity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// タイトルUI設定
/// </summary>
public class TitleUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SkeletonAnimation _skeleton;
    [SerializeField] private TitleCameraController _cameraController;

    [Header("UI Settings")]
    [SerializeField] private GameObject _gamePlayButton;
    [SerializeField] private Text _titleText;
    [SerializeField] private Text _gamePlayButtonText;
    [SerializeField] private GameObject _playerObject;

    [Header("Show Image Time")]
    [SerializeField] private float _showTitleTextTime = 6f;
    [SerializeField] private float _showCharacterTime = 7f;
    [SerializeField] private float _showButtonTextTime = 8f;
    [SerializeField] private float _showPlayer = 2f;

    private bool _isShown;

    private float _timer;

    private void Awake()
    {
        _skeleton = GetComponent<SkeletonAnimation>();
    }

    void Start()
    {
        _gamePlayButton.SetActive(false);
        _titleText.gameObject.SetActive(false);
        _gamePlayButtonText.gameObject.SetActive(false);
        _playerObject.SetActive(false);
    }

    private void Update()
    {
        if (_isShown) return;

        _timer += Time.deltaTime;

        if(_timer >= _showTitleTextTime)
        {
            _titleText.gameObject.SetActive(true);
        }

        if( _timer >= _showCharacterTime)
        {
            _playerObject.SetActive(true);
        }
        if(_timer >= _showButtonTextTime)
        {
            _gamePlayButtonText.gameObject.SetActive(true);
            _gamePlayButton.SetActive(true);

            _cameraController.CanMoving = true;

            _isShown = true;
        }

    }

    /// <summary>
    /// ゲームスタートボタンクリック時の処理
    /// </summary>
    public void OnClickGameStartButton()
    {
        SceneManager.LoadScene("MainScene");
    }

}
