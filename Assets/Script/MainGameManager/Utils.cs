using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public static class Utils
{
    public static string GetTextTimer(float timer)
    {
        int seconds = (int)timer % 60;
        int minutes = (int)timer / 60;

        return minutes.ToString("00") + " " + seconds.ToString("00"); 
    }


    /// <summary>
    /// 当たり判定あるかを判定する処理
    /// </summary>
    /// <param name="TilemapCollider2D"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool IsColliderTile(Tilemap TilemapCollider2D, Vector3 position)
    {
        //セルの位置に変更処理
        Vector3Int cellPosition = TilemapCollider2D.WorldToCell(position);

        if (TilemapCollider2D.GetTile(cellPosition))
        {
            return true;
        }

        return false;
    }

    //アルファ値設定
    public static void SetAlpha(Graphic graphic, float alpha)
    {
        //元のカラー
        Color color = graphic.color;

        //アルファ値設定
        color.a = alpha;
        graphic.color = color;
    }

    //アルファ値設定（ボタン）
    public static void SetAlpha(Button button, float alpha)
    {
        //ボタン自体
        SetAlpha(button.image, alpha);
        //子オブジェクトすべて
        foreach(var item in button.GetComponentsInChildren<Graphic>())
        {
            SetAlpha(item, alpha);
        }
    }
}
