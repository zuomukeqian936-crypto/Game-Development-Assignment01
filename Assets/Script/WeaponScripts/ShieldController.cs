using DG.Tweening;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class ShieldController : BaseWeapon
{
    //プレイヤーからの距離
    private const float _radius = 1f;
    //現在の角度
    public float _angle;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //フワッとひゅおうじする
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f), 0.3f).SetEase(Ease.OutBounce);
    }

    // Update is called once per frame
    void Update()
    {
        //角度更新
        _angle -= _weaponSpawnerStats.MoveSpeed * Time.deltaTime;

        //Cos関数にラジアン角を指定すると、Xの座標を返してくれる、radiusをかけてワールド座標に変換する
        float x = Mathf.Cos(_angle * Mathf.Deg2Rad) * _radius;
        //Sin関数にラジアン角を指定すると、Yの座標を返してくれる、radiusをかけてワールド座標に変換する
        float y = Mathf.Sin(_angle * Mathf.Deg2Rad) * _radius;

        //ポジション更新
        transform.position = transform.root.position + new Vector3(x, y, 0);
    }

    //トリガーが突破したとき
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //敵以外
        if (!collision.gameObject.TryGetComponent<EnemyController>(out var enemy)) return;

        //反対側へ跳ね返す
        Vector3 forward = enemy.transform.position - transform.root.position;
        enemy.GetComponent<Rigidbody2D>().AddForce(forward.normalized * 5);

        DefaultAttackEnemy(collision);
    }
}
