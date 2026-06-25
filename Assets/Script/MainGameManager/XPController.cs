using Dreamteck;
using UnityEngine;

public class XPController : MonoBehaviour
{
    private GameSceneDirector _gameSceneDirector;
    private Rigidbody2D _rb2D;
    private SpriteRenderer _spriteRenderer;

    //経験値
    private float _xp;
    //消滅時間
    private float _aliveTimer = 50f;
    private float _fadeTime = 10f;

    //初期化
    public void Init(GameSceneDirector gameSceneDirector, float xp)
    {
        this._gameSceneDirector = gameSceneDirector;
        this._xp = xp;

        _rb2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        float dist = Vector2.Distance(transform.position, _gameSceneDirector._playerController.transform.position);
        //所得範囲内で吸い込まれる処理
        if(dist < _gameSceneDirector._playerController._characterStats.PickUpRange)
        {
            //少し早く移動
            float speed = _gameSceneDirector._playerController._characterStats.MoveSpeed * 1.1f;
            Vector2 forward = _gameSceneDirector._playerController.transform.position - transform.position;
            _rb2D.position += forward.normalized * speed * Time.deltaTime;
        }
    }

    //トリガーが衝突したとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.TryGetComponent<PlayerController>(out var player)) return;
        player.GetXP(_xp);
        Destroy(gameObject);
    }
}
