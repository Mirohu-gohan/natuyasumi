using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float maxTurnAngle = 20f; // 旋回最大角度

    private Transform _target;
    private float _bulletRadius;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void Start()
    {
        // 弾の半径を取得
        _bulletRadius = transform.localScale.x * 0.5f;
    }

    void Update()
    {
        if (_target != null)
        {
            HandleTracking();
        }
        else
        {
            // ターゲットが消えたらそのまま直進
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }
    }

    private void HandleTracking()
    {
        // ターゲットの方向を計算
        Vector3 directionToTarget = (_target.position - transform.position).normalized;

        // 目標の回転を計算（LookAtは使わず）
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // 徐々にターゲットに向かって回転
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxTurnAngle * Time.deltaTime);

        // 前方に移動
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        // 衝突判定
        Vector3 aabbCenter = _target.position;
        // EnemyのlocalScaleを取得して、extentsを計算
        Vector3 aabbExtents = _target.localScale * 0.5f;

        if (CollisionManager.CheckSphereAABB(transform.position, _bulletRadius, aabbCenter, aabbExtents))
        {
            _target.GetComponent<Enemy>()?.TakeDamage();
            Destroy(gameObject);
        }
    }
}