using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelLevelUpController : MonoBehaviour
{
    [SerializeField] private List<Button> _buttonLevelUps;
    [SerializeField] private Button _buttonCancel;

    private GameSceneDirector _gameSceneDirector;

    //選択カーソル
    private int selectButtonCursor;
    //表示中のボタン
    private List<Button> _dispButtons;
    
    //初期化
    public void Init(GameSceneDirector gameSceneDirector)
    {
        this._gameSceneDirector = gameSceneDirector;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //レベルアップ時のボタンの設定
    private void SetButtonLvUp(Button button, int lv, string name, string desc, Sprite icon)
    {
        Image image = button.transform.Find("Item Image").GetComponent<Image>();
        Text itemName = button.transform.Find("Item Name Text").GetComponent<Text>();
        Text level = button.transform.Find("Level Text").GetComponent<Text>();
        Text help = button.transform.Find("Description Text").GetComponent<Text>();

        //デバッグ用
        //if (image == null || itemName == null || level == null || help == null)
        //{
        //    Debug.LogError($"UI要素が見つかりません: {button.name}");
        //    if (image == null)
        //    {
        //        Debug.LogError("imageが見つかりません");
        //    }
        //    if(itemName == null)
        //    {
        //        Debug.LogError("itemNameが見つかりません");
        //    }
        //    if(level == null)
        //    {
        //        Debug.LogError("levelが見つかりません");
        //    }
        //    if(help == null)
        //    {
        //        Debug.LogError("helpが見つかりません");
        //    }
            
        //    return;
        //}

        image.sprite = icon;
        itemName.text = name;
        help.text = desc;
        //レベルの表示を少し変える
        level.text = "LV :" + lv;
        level.color = Color.white;

        //初期装備
        if(1  == lv)
        {
            level.text = "NEW !!";
            level.color = Color.yellow;
        }

        button.gameObject.SetActive(true);
    }

    //キャンセルボタン
    public void OnClickCancel()
    {
        gameObject.SetActive(false);
        _gameSceneDirector.PlayGame();
    }

    //レベルアップパネル表示
    public void DispPanel(List<WeaponSpawnerStats> items)
    {
        //アイテムがないとき
        _buttonCancel.gameObject.SetActive(false);

        //表示中のボタン
        _dispButtons = new List<Button>();
        for(int i = 0; i < _buttonLevelUps.Count; i++)
        {
            //今回生成するボタン
            Button button = _buttonLevelUps[i];

            //ボタン初期化
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();

            //表示するアイテムがなければ次へ
            if (items.Count - 1 < i) continue;

            //今回設定するアイテム
            WeaponSpawnerStats item = items[i];

            //押された時の処理
            button.onClick.AddListener(() =>
            {
                _gameSceneDirector.PlayGame(new BonusData(item));
                gameObject.SetActive(false);
            });

            //ボタンのデータ
            int lv = item.Lv;
            string name = item.Name;
            string desc = item.Description;
            Sprite icon = item._icon;

            //ボタン作成
            SetButtonLvUp(button, lv, name, desc, icon);
            _dispButtons.Add(button);
        }

        //カーソルリセット
        selectButtonCursor = 0;

        //選べるボタンなし
        if(1 > items.Count)
        {
            _buttonCancel.gameObject.SetActive(true);
            //デフォルトで選択状態にする
            _buttonCancel.Select();
        }
        //一つ目の項目を選択状態にする
        else
        {
            _dispButtons[0].Select();
        }

        //前面に表示
        transform.SetAsFirstSibling();

        //パネル表示
        gameObject.SetActive(true);
    }

    //レベルアップパネルで必要なアイテム数
    public int GetButtonCount()
    {
        return _buttonLevelUps.Count;
    }
}
