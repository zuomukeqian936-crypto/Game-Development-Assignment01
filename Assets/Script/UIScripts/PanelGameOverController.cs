using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelGameOverController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResultSceneDirector _resultSceneDirector;
    private GameSceneDirector _gameSceneDirector;

    [Header("UI Settings")]
    [SerializeField] private Image _panelGameOverBG;
    [SerializeField] private Text _textSurvivedResult;
    [SerializeField] private Text _textLevelResult;
    [SerializeField] private Text _textEnemyuResult;

    [SerializeField] private Text _textWeaponNameTitle;
    [SerializeField] private Text _textWeaponLevelTitle;
    [SerializeField] private Text _textWeaponDamageTitle;
    [SerializeField] private Text _textWeaponTimeTitle;
    [SerializeField] private Text _TextWeaponDPSTitle;

    //終了ボタン
    [SerializeField] private Button _buttonDown;
    //パネル上の全テキスト
    private List<Text> _resultTexts;

    //初期化
    public void Init(ResultSceneDirector resultSceneDirector)
    {
        if(_gameSceneDirector == null)
        {
            _gameSceneDirector = FindAnyObjectByType<GameSceneDirector>();
        }
        this._resultSceneDirector = resultSceneDirector;
        _resultTexts = new List<Text>();

        //全テキスト取得
        foreach(var item in gameObject.GetComponentsInChildren<Text>())
        {
            _resultTexts.Add(item);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //テキストを複製して新しいy座標をセット
    private Text DuplicateText(Text parent, string dispText, float y)
    {
        GameObject obj = Instantiate(parent.gameObject, transform);
        Text text = obj.GetComponent<Text>();  
        text.color = Color.white;
        text.text = dispText;

        //y座標セット
        Vector3 pos = text.rectTransform.anchoredPosition;
        pos.y = y;
        text.rectTransform.anchoredPosition = pos;

        return text;
    }

    //パネル表示
    public void DispPanel(List<BaseWeaponSpawner> weaponSpawners)
    {
        //生成時間
        _textSurvivedResult.text = Utils.GetTextTimer(_gameSceneDirector._gameTimer);
        //レベル
        _textLevelResult.text = "" + _gameSceneDirector._playerController._characterStats.Lv;
        //倒した敵の数
        _textEnemyuResult.text = "" + _gameSceneDirector._defeatedEnemyCount;

        //スタート位置
        float y = _textWeaponNameTitle.rectTransform.anchoredPosition.y;
        float h = _textWeaponNameTitle.rectTransform.rect.height;

        //武器のリザルトを1行ずつ生成
        foreach(var item in weaponSpawners)
        {
            //表示位置
            y -= h;

            //武器名
            Text text = DuplicateText(_textWeaponNameTitle, item._weaponStats.Name, y);
            _resultTexts.Add(text);

            //レベル
            text = DuplicateText(_textWeaponLevelTitle, "" + item._weaponStats.Lv, y);
            _resultTexts.Add(text);

            //ダメージ
            text = DuplicateText(_textWeaponDamageTitle, "" + (int)item._totalDamage, y);
            _resultTexts.Add(text);

            //時間
            text = DuplicateText(_textWeaponTimeTitle, Utils.GetTextTimer(item._totalTimer), y);

            //DPS
            int sec = (int)(item._totalTimer + 1);
            int dps = (int)(item._totalDamage + sec);
            text = DuplicateText(_TextWeaponDPSTitle, "" + dps, y);
            _resultTexts.Add(text);
        }
        //順番に表示
        Sequence seq = DOTween.Sequence();
        float dispTime = 1.5f;

        //BG
        Utils.SetAlpha(_panelGameOverBG, 0);
        seq.Append(_panelGameOverBG.DOFade(1, dispTime));

        //パネル
        Image panelGameOver = gameObject.GetComponent<Image>();
        Utils.SetAlpha(panelGameOver, 0);
        seq.Append(panelGameOver.DOFade(1, dispTime));

        //全テキスト
        for (int i = 0; i < _resultTexts.Count; i++)
        {
            var item = _resultTexts[i];
            Utils.SetAlpha(item, 0);

            //1つ目のテキスト
            if (0 == i)
            {
                seq.Append(item.DOFade(1, dispTime));
            }
            //それ以外は一つ目に合わせる
            else
            {
                seq.Join(item.DOFade(1, dispTime));
            }
        }

        //閉じるボタンと子オブジェクト
        Utils.SetAlpha(_buttonDown, 0);

        //表示し終わったらリスナーを登録
        seq.Append(_buttonDown.image.DOFade(1, dispTime)
            .OnComplete(() =>
            {
                _buttonDown.onClick.AddListener(_resultSceneDirector.LoadSceneTitle);
                _buttonDown.Select();
            }));

        foreach(var item in _buttonDown.GetComponentsInChildren<Graphic>())
        {
            seq.Join(item.DOFade(1, dispTime));
        }

        //再生
        seq.Play().SetUpdate(true);

        //前面に表示
        _panelGameOverBG.transform.SetAsFirstSibling();
        transform.SetAsFirstSibling();

        //パネル表示
        _panelGameOverBG.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    
}
