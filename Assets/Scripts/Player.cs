using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    // �X�N���v�g�̊O�����璲���ł���p�����[�^
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("LockOn")]
    [SerializeField] private float lockOnRange = 30f;
    [SerializeField] private float lockOnAngle = 60f; // ����p�̔���

    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireAngleOffset = 5f;

    // �v���C�x�[�g�ϐ�
    private float _currentYRotation = 0f;
    private float _currentXRotation = 0f;
    private List<GameObject> _lockedOnEnemies = new List<GameObject>();
    private GameManager _gameManager;

    void Start()
    {
        // FindObjectOfType�̑���ɐ�������Ă���FindFirstObjectByType���g�p
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
        // W/S�L�[�őO�i�E���
        float verticalInput = Input.GetAxis("Vertical");
        transform.position += transform.forward * verticalInput * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        // ���E�L�[��Y������A�㉺�L�[��X������
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("VerticalRotation"); // Input Manager�ŕʓr�ݒ肪�K�v

        _currentYRotation += horizontalInput * rotationSpeed * Time.deltaTime;
        _currentXRotation += verticalInput * rotationSpeed * Time.deltaTime;
        _currentXRotation = Mathf.Clamp(_currentXRotation, -80f, 80f); // �㉺����̊p�x����

        // Y����]��X����]���������čŏI�I�ȉ�]��K�p
        Quaternion yRotation = Quaternion.Euler(0, _currentYRotation, 0);
        Quaternion xRotation = Quaternion.Euler(_currentXRotation, 0, 0);
        transform.rotation = yRotation * xRotation;
    }

    private void HandleLockOn()
    {
        // Z�L�[�������Ƀ��b�N�I���Ώۂ��X�V
        if (Input.GetKey(KeyCode.Z))
        {
            UpdateLockOnTargets();
        }
        // Z�L�[�𗣂����ۂɒe�𔭎�
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

            // ���ς��g���Ď���p���𔻒�
            float dotProduct = Vector3.Dot(transform.forward, directionToEnemy);

            if (dotProduct > Mathf.Cos(lockOnAngle * Mathf.Deg2Rad) && distanceToEnemy <= lockOnRange)
            {
                potentialTargets.Add((enemy, distanceToEnemy));
            }
        }

        // �������߂����Ƀ\�[�g���āA�ő�8�̂ɍi��
        _lockedOnEnemies = potentialTargets
            .OrderBy(t => t.distance)
            .Take(8)
            .Select(t => t.enemy)
            .ToList();

        // ���b�N�I�����ꂽ�G�̐F��ԂɁA����ȊO�����ɖ߂�
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

        // ���b�N�I�������G�̐������e�𐶐����A���ꂼ��Ƀ^�[�Q�b�g��ݒ�
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