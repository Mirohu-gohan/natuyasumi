using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    // スクリプトの外部から調整できるパラメータ
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("LockOn")]
    [SerializeField] private float lockOnRange = 30f;
    [SerializeField] private float lockOnAngle = 60f; // 視野角の半分

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireAngleOffset = 5f;

    // プライベート変数
    private float _currentYRotation = 0f;
    private float _currentXRotation = 0f;
    private List<GameObject> _lockedOnEnemies = new List<GameObject>();
    private GameManager _gameManager;

    void Start()
    {
        // FindObjectOfTypeの代わりに推奨されているFindFirstObjectByTypeを使用
        _gameManager = FindFirstObjectByType<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleLockOn();
    }

    private void HandleMovement()
    {
        // W/Sキーで前進・後退
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += transform.forward * verticalInput * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        // 左右キーでY軸旋回、上下キーでX軸旋回
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("VerticalRotation"); // Input Managerで別途設定が必要

        _currentYRotation += horizontalInput * rotationSpeed * Time.deltaTime;
        _currentXRotation += verticalInput * rotationSpeed * Time.deltaTime;
        _currentXRotation = Mathf.Clamp(_currentXRotation, -80f, 80f); // 上下旋回の角度制限

        // Y軸回転とX軸回転を合成して最終的な回転を適用
        Quaternion yRotation = Quaternion.Euler(0, _currentYRotation, 0);
        Quaternion xRotation = Quaternion.Euler(_currentXRotation, 0, 0);
        transform.rotation = yRotation * xRotation;
    }

    private void HandleLockOn()
    {
        // Zキー押下中にロックオン対象を更新
        if (Input.GetKey(KeyCode.Z))
        {
            UpdateLockOnTargets();
        }
        // Zキーを離した際に弾を発射
        else if (Input.GetKeyUp(KeyCode.Z))
        {
            FireBullets();
            ClearLockOnTargets();
        }
    }

    private void UpdateLockOnTargets()
    {
        _lockedOnEnemies.Clear();
        var allEnemies = _gameManager.GetAllEnemies();

        var potentialTargets = new List<(GameObject enemy, float distance)>();

        foreach (var enemy in allEnemies)
        {
            if (enemy == null) continue;

            Vector3 directionToEnemy = (enemy.transform.position - transform.position).normalized;
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            // 内積を使って視野角内を判定
            float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);

            if (dotProduct > Mathf.Cos(lockOnAngle * Mathf.Deg2Rad) && distanceToEnemy <= lockOnRange)
            {
                potentialTargets.Add((enemy, distanceToEnemy));
            }
        }

        // 距離が近い順にソートして、最大8体に絞る
        _lockedOnEnemies = potentialTargets
            .OrderBy(t => t.distance)
            .Take(8)
            .Select(t => t.enemy)
            .ToList();

        // ロックオンされた敵の色を赤に、それ以外を元に戻す
        foreach (var enemy in allEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().SetLockedOn(_lockedOnEnemies.Contains(enemy));
            }
        }
    }

    private void ClearLockOnTargets()
    {
        foreach (var enemy in _lockedOnEnemies)
        {
            if (enemy != null)
            {
                enemy.GetComponent<Enemy>().SetLockedOn(false);
            }
        }
        _lockedOnEnemies.Clear();
    }

    private void FireBullets()
    {
        if (_lockedOnEnemies.Count == 0) return;

        // ロックオンした敵の数だけ弾を生成し、それぞれにターゲットを設定
        for (int i = 0; i < _lockedOnEnemies.Count; i++)
        {
            if (_lockedOnEnemies[i] == null) continue;

            GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, transform.rotation);
            Bullet bulletController = bulletInstance.GetComponent<Bullet>();

            if (bulletController != null)
            {
                bulletController.SetTarget(_lockedOnEnemies[i].transform);
            }
        }
    }
}