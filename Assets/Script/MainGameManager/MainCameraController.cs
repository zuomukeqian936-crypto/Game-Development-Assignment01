using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MainCameraController : MonoBehaviour
{
    [Header("Back Ground Color")]
    [SerializeField] private Color _backGourndFinishColor;
    private Image _imageColor;

    [Header("Chenge Color Settings")]
    [SerializeField] private float _playerChengeColorTime = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _imageColor = GetComponent<Image>();

        HandleColorChange();
    }

    /// <summary>
    /// 色変更処理
    /// </summary>
    private void HandleColorChange()
    {
        _imageColor.DOColor(_backGourndFinishColor, _playerChengeColorTime)
        .SetEase(Ease.InOutQuad);

        StartCoroutine(SetFadeColor());
    }

    private IEnumerator SetFadeColor()
    {
        yield return new WaitForSeconds(_playerChengeColorTime);
        _imageColor.DOFade(0f, 0f);
    }

}


