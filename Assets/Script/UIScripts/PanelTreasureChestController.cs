using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PanelTreasureChestController : MonoBehaviour
{
    [SerializeField] private Image _imageClose;
    [SerializeField] private Image _imageOpen;
    [SerializeField] private Image _imageItem;
    [SerializeField] private Image _imageBackFX;
    [SerializeField] private Image _imageBackFXShiny;
    [SerializeField] private Button _buttonOpen;
    [SerializeField] private Button _buttonClose;
    [SerializeField] private Text _textDescription;

    private GameSceneDirector _gameSceneDirector;

    //取得アイテム
    private ItemData _itemData;
    //取得アイテム画像初期位置
    private Vector3 imageItemInitPos;
    //宝箱画像初期スケール
    private Vector3 imageCloseInitScale;
    //取得アイテムアニメーション位置
    private Vector2 itemTargetPos = new Vector2(0, 70);
    
    //初期化
    public void Init(GameSceneDirector gameSceneDirector)
    {
        this._gameSceneDirector = gameSceneDirector;

        //初期位置
        imageItemInitPos = _imageItem.rectTransform.anchoredPosition;
        imageCloseInitScale = _imageClose.rectTransform.localScale;
    }

    //宝箱パネルを表示
    public void DispPanel(ItemData itemData)
    {
        //今回取得したアイテム
        _itemData = itemData;

        //場所などの初期化
        _imageItem.rectTransform.anchoredPosition = imageItemInitPos;
        _imageClose.rectTransform.localScale = imageCloseInitScale;
        _imageClose.rectTransform.localEulerAngles = Vector3.zero;
        _imageBackFX.rectTransform.anchoredPosition = itemTargetPos;
        _imageBackFXShiny.rectTransform.anchoredPosition = itemTargetPos;

        //アイテム画像
        _imageItem.sprite = _itemData.Icon;
        Utils.SetAlpha(_imageItem, 0);

        //アイテム説明
        _textDescription.text = _itemData.Description;
        Utils.SetAlpha(_textDescription, 0);

        //閉じた宝箱
        _imageClose.gameObject.SetActive(true);
        Utils.SetAlpha(_imageClose, 1);

        //開いた宝箱
        _imageOpen.gameObject.SetActive(false);
        Utils.SetAlpha(_imageOpen, 1);

        //オープンボタン
        _buttonOpen.gameObject.SetActive(true);
        Utils.SetAlpha( _buttonOpen, 1);

        //クローズボタン
        _buttonClose.gameObject.SetActive(false);
        Utils.SetAlpha(_buttonClose, 0);

        //演出
        Utils.SetAlpha(_imageBackFX, 0);
        Utils.SetAlpha(_imageBackFXShiny, 0);

        //ボタンを選択状態にする
        _buttonOpen.Select();

        //パネル本体
        gameObject.SetActive(true);
    }

    //オープンボタンクリック
    public void OnClickOpen()
    {
        //ボタン非表示
        _buttonOpen.gameObject.SetActive(false);

        //閉じた宝箱
        Transform image = _imageClose.transform;

        //アニメーション設定
        Vector3 punchScale = new Vector3(1.5f, 1.5f, 0);
        Vector3 punchRotate = new Vector3(0, 0, 30f);
        Vector3 endScale = new Vector3(1.5f, 0.5f, 0f);
        float duration = 0.5f;

        //シーケンスでアニメーションを順番に再生
        Sequence seq = DOTween.Sequence();

        //宝箱が弾む(大きさ、時間、振動、弾力性）
        seq.Append(image.DOPunchScale(punchScale, duration, 5,1));

        //宝箱の回転アニメーション(joinで同時に再生）
        seq.Join
        (
            image.DOPunchRotation(punchRotate, duration)
            //アニメーションの始まりと終わりがゆっくり中間が早くなる
            .SetEase(Ease.InOutQuad)
            //繰り返す回数とタイプ
            .SetLoops(1, LoopType.Yoyo)
        );

        //つぶれるアニメーション
        seq.Append
        (
            image.DOScale(endScale, duration)
            .SetEase(Ease.OutBounce)
        );

        //再生
        seq.Play().SetUpdate(true).OnComplete(() => dispItem());
    }

    //回転のみ関数化
    private void DoRotatieLoops(Image image, int dir = 1)
    {
        image.transform
        .DORotate(new Vector3(0, 0, 360f) * dir, 60f, RotateMode.FastBeyond360)
        .SetUpdate(true)
        .SetEase(Ease.Linear)
        .SetLoops(-1, LoopType.Restart);
    }

    //アイテム表示
    private void dispItem()
    {
        //宝箱表示
        _imageClose.gameObject.SetActive(false);
        _imageOpen.gameObject.SetActive(true);

        //取得アイテム
        RectTransform image = _imageItem.rectTransform;

        //アイテム表示時間
        float itemDuration = 1f;

        //演出表示時間
        float fxDuration = itemDuration / 2;

        //アニメーションを順番に再生
        Sequence seq = DOTween.Sequence();

        //開いた宝箱がフェードアウト（全体の開始を遅らせる）
        seq.Append(_imageOpen.DOFade(0, itemDuration).SetDelay(0.5f));

        //アイテムフェードイン　＆　移動
        seq.Append(_imageItem.DOFade(0, itemDuration));
        seq.Join(image.DOAnchorPos(itemTargetPos, itemDuration));

        //演出フェードイン
        seq.Append(_imageBackFX.DOFade(1, fxDuration));
        seq.Join(_imageBackFXShiny.DOFade(0.8f, fxDuration));

        //説明フェードイン
        seq.Append
        (
            _textDescription.DOFade(1, fxDuration)
            .OnComplete(() => _buttonClose.gameObject.SetActive(true))
        );

        //閉じるボタンと子オブジェクトをフェードイン
        seq.Append(
            _buttonClose.image.DOFade(1, fxDuration)
            .OnComplete(() => _buttonClose.Select())
            );

        foreach(var item in _buttonClose.GetComponentsInChildren<Graphic>())
        {
            seq.Join(item.DOFade(1, fxDuration));
        }

        //開始
        seq.Play().SetUpdate(true);

        //無限ループ系はシーケンスと別で動かす
        DoRotatieLoops(_imageBackFX);
        DoRotatieLoops(_imageBackFXShiny, -1);
    }

    //クローズボタン
    public void OnClickClose()
    {
        //ループ系を止める
        _imageBackFX.DOKill();
        _imageBackFXShiny.DOKill();
        //パネル非表示
        gameObject.SetActive(false);
        //ゲーム開始
        _gameSceneDirector.PlayGame(new BonusData(_itemData));
    }
}
