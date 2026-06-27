using UnityEngine;

public class PinwheelController : BaseWeapon
{
    //反射回数
    private int reflectionCount = 5;
    //カメラ表示範囲
    private Vector2 cameraSize;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ランダムな方向に向かって飛ばす
        _forward = new Vector2(Random.Range(-1f, -1f), Random.Range(-1f, 1f));
        _forward.Normalize();

        //カメラの表示範囲（半分）
        float aspect = Screen.width / (float)Screen.height;
        cameraSize = new Vector2(Camera.main.orthographicSize * aspect, Camera.main.orthographicSize);
    }

    // Update is called once per frame
    void Update()
    {
        //反射回数消化
        if(0 > reflectionCount)
        {
            Destroy(gameObject);
            return;
        }

        //壁際で移動を反転
        Vector2 camera = Camera.main.transform.position;
        Vector2 start = new Vector2(camera.x - cameraSize.x, camera.y - cameraSize.y);
        Vector2 end = new Vector2(camera.x + cameraSize.x, camera.y + cameraSize.y);
        Vector2 pos = _rb2D.position;

        //画面外の判定
        if(pos.x < start.x || end.x < pos.x)
        {
            _forward.x *= -1;
            reflectionCount--;
        }

        //回転
        transform.Rotate(new Vector3(0, 0, 1000 * Time.deltaTime));

        //移動
        _rb2D.position += _forward * _weaponSpawnerStats.MoveSpeed * Time.deltaTime;

    }

    //トリガーが衝突したときに呼ばれる処理
    private void OnTriggerEnter2D(Collider2D collision)
    {
        DefaultAttackEnemy(collision);
    }
}
