using UnityEngine;
using UnityEngine.Tilemaps;

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
    public static bool IsColliderTile(Tilemap TilemapCollider2D, Vector2 position)
    {
        //セルの位置に変更処理
        Vector3Int cellPosition = TilemapCollider2D.WorldToCell(position);

        if (TilemapCollider2D.GetTile(cellPosition))
        {
            return true;
        }

        return false;
    }
}
