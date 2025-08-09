using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float maxTurnAngle = 20f; // ����ő�p�x

    private Transform _target;
    private float _bulletRadius;

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    void Start()
    {
        // �e�̔��a���擾
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
            // �^�[�Q�b�g���������炻�̂܂ܒ��i
            transform.position += transform.forward * bulletSpeed * Time.deltaTime;
        }
    }

    private void HandleTracking()
    {
        // �^�[�Q�b�g�̕������v�Z
        Vector3 directionToTarget = (_target.position - transform.position).normalized;

        // �ڕW�̉�]���v�Z�iLookAt�͎g�킸�j
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

        // ���X�Ƀ^�[�Q�b�g�Ɍ������ĉ�]
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, maxTurnAngle * Time.deltaTime);

        // �O���Ɉړ�
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        // �Փ˔���
        Vector3 aabbCenter = _target.position;
        // Enemy��localScale���擾���āAextents���v�Z
        Vector3 aabbExtents = _target.localScale * 0.5f;

        if (CollisionManager.CheckSphereAABB(transform.position, _bulletRadius, aabbCenter, aabbExtents))
        {
            _target.GetComponent<Enemy>()?.TakeDamage();
            Destroy(gameObject);
        }
    }
}