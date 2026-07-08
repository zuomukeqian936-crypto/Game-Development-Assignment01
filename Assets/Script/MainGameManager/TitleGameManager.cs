using DG.Tweening;
using Spine.Unity;
using System.Collections;
using Unity.VectorGraphics;
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
    [SerializeField] private Image _backGroundImage;

    [Header("Color Settings")]
    [SerializeField] private Color _backGourndFadeInColor;
    [SerializeField] private Color _backGroundFadeOutColor;

    [Header("Show Image Time")]
    [SerializeField] private float _backgroundImageFadeInTime = 5f;
    [SerializeField] private float _backgroundImageFadeOutTime = 3f;
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

        HandleColorChange();
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
    /// フェードイン演出
    /// </summary>
    private void HandleColorChange()
    {
        _backGroundImage.DOColor(_backGourndFadeInColor, _backgroundImageFadeInTime)
        .SetEase(Ease.InOutQuad);

        StartCoroutine(SetFadeColor());
    }

    private IEnumerator SetFadeColor()
    {
        yield return new WaitForSeconds(_backgroundImageFadeInTime);
        _backGroundImage.DOFade(0f, 0f);
    }

    /// <summary>
    /// ゲームスタートボタンクリック時フェードアウトとシーン移動演出処理
    /// </summary>
    public void OnClickGameStartButton()
    {
        _backGroundImage.DOFade(1f, _backgroundImageFadeOutTime)
            .OnComplete(() =>
            {
                SceneManager.LoadScene("MainScene");
            });
    }

}
