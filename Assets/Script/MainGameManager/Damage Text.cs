using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DamageText : MonoBehaviour
{
    [Header("Text Destroy Time")]
    [SerializeField] private float _destroyTime = 1;

    private GameObject _target;

    void Start()
    {
        //ダメージ受けるとテキストが膨らんで消える処理
        transform.DOScale(new Vector2(1, 1), _destroyTime / 2)
            .SetRelative()
            .OnComplete(() =>
            {
                transform.DOScale(new Vector2(0, 0), _destroyTime / 2)
                .OnComplete(() => Destroy(gameObject));
            });
    }

    void Update()
    {
        if (!_target) return;

        UpdateTextPosition();
    }

    /// <summary>
    /// テキスト位置更新処理
    /// </summary>
    private void UpdateTextPosition()
    {
        Vector3 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, _target.transform.position);
        transform.position = pos;
    }

    /// <summary>
    /// ダメージ表示処理　（テキストの色：敵は元のカラー、プレイヤーは赤色で表示）
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damage"></param>
    public void Init(GameObject target, float damage)
    {
        _target = target;

        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        text.text = "" + (int)damage;

        if (target.GetComponent<PlayerController>())
        {
            text.color = Color.red;
        }
    }

    /// <summary>
    /// レベルアップや宝箱の表示したときに強制的に削除処理
    /// </summary>
    public void ForceDestroy()
    {
        Destroy(gameObject);
    }
}
