using Dreamteck;
using UnityEngine;

public class XPController : MonoBehaviour
{
    private GameSceneDirector _gameSceneDirector;
    private GameObject _player;
    private Rigidbody2D _rb2D;
    private SpriteRenderer _spriteRenderer;

    //経験値
    private float _xp;
    //消滅時間
    private float _aliveTimer = 50f;
    private float _fadeTime = 10f;

    //初期化
    private void Init(GameSceneDirector gameSceneDirector, float xp)
    {
        this._gameSceneDirector = gameSceneDirector;
        this._xp = xp;

        _rb2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _player = GameObject.FindWithTag("Player");
    }
    
    void Update()
    {
        //ゲーム停止中
        if (!_gameSceneDirector.enabled) return;

        //タイマー消化で消え始める
        _aliveTimer -= Time.deltaTime;

        if(0 > _aliveTimer)
        {
            //アルファ値を設定
            Color color = _spriteRenderer.color;
            color.a -= 1.0f / _fadeTime * Time.deltaTime;
            _spriteRenderer.color = color;

            //見えなくなったら消す処理
            if(0 >= color.a)
            {
                Destroy(gameObject);
                return;
            }
        }

        //プレイヤーとの距離
        float dist = Vector2.Distance(transform.position, _player.transform.position);
        //所得範囲内で吸い込まれる処理
        //if(dist < _gameSceneDirector._)
    }
}
