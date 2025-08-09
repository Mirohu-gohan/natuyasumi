using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // �X�N���v�g�̊O�����璲���ł���p�����[�^
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 24;
    [SerializeField] private float enemySpacing = 1f; // �G���m�̊Ԋu
    [SerializeField] private Vector3 spawnCenter = new Vector3(0, 0, 30f);

    private List<GameObject> _enemies = new List<GameObject>();

    void Start()
    {
        SpawnEnemies();
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            // ���ɕ��ׂ邽�߂̈ʒu���v�Z
            // X��������enemySpacing�̊Ԋu�ŕ��ׂ��
            Vector3 spawnPosition = spawnCenter + new Vector3(i * enemySpacing, 0, 0);

            // �G�𐶐����A���X�g�ɒǉ�
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            _enemies.Add(enemy);
        }
    }

    /// <summary>
    /// �V�[�����̑S�Ă̓G�̃��X�g��Ԃ��܂��B
    /// </summary>
    public List<GameObject> GetAllEnemies()
    {
        return _enemies;
    }
}