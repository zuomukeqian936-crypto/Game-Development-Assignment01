using UnityEngine;

public class TresureChestController : MonoBehaviour
{
    private GameSceneDirector _gameSceneDirector;

    //初期化
    public void Init(GameSceneDirector gameSceneDirector)
    {
        this._gameSceneDirector = gameSceneDirector;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<PlayerController>(out var player)) return;

        _gameSceneDirector.DispPanelTreasureChest();
        Destroy(gameObject);
    }
}
