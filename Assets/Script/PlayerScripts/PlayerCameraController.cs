using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private GameSceneDirector _gameSceneDirector;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_gameSceneDirector != null) return;

        _gameSceneDirector = FindAnyObjectByType<GameSceneDirector>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    /// <summary>
    /// カメラ追随処理
    /// </summary>
    private void MoveCamera()
    {
        Vector3 pos = transform.position;
        pos.z = Camera.main.transform.position.z;

        if (pos.x < _gameSceneDirector._TileMapStart.x)
        {
            pos.x = _gameSceneDirector._TileMapStart.x;
        }
        if (pos.y < _gameSceneDirector._TileMapStart.y)
        {
            pos.y = _gameSceneDirector._TileMapStart.y;
        }
        if (_gameSceneDirector._TileMapEnd.x < pos.x)
        {
            pos.x = _gameSceneDirector._TileMapEnd.x;
        }
        if (_gameSceneDirector._TileMapEnd.y < pos.y)
        {
            pos.y = _gameSceneDirector._TileMapEnd.y;
        }

        Camera.main.transform.position = pos;
    }
}
