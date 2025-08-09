using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // スクリプトの外部から調整できるパラメータ
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 24;
    [SerializeField] private float enemySpacing = 1f; // 敵同士の間隔
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
            // 一列に並べるための位置を計算
            // X軸方向にenemySpacingの間隔で並べる例
            Vector3 spawnPosition = spawnCenter + new Vector3(i * enemySpacing, 0, 0);

            // 敵を生成し、リストに追加
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            _enemies.Add(enemy);
        }
    }

    /// <summary>
    /// シーン内の全ての敵のリストを返します。
    /// </summary>
    public List<GameObject> GetAllEnemies()
    {
        return _enemies;
    }
}