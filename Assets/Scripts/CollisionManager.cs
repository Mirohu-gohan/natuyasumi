using UnityEngine;

public static class CollisionManager
{
    /// <summary>
    /// 球体とAABB（直方体）の衝突を判定します。
    /// </summary>
    /// <param name="spherePos">球体の中心座標</param>
    /// <param name="sphereRadius">球体の半径</param>
    /// <param name="aabbCenter">AABBの中心座標</param>
    /// <param name="aabbExtents">AABBの半分のサイズ</param>
    /// <returns>衝突していればtrue</returns>
    public static bool CheckSphereAABB(Vector3 spherePos, float sphereRadius, Vector3 aabbCenter, Vector3 aabbExtents)
    {
        Vector3 closestPoint = new Vector3(
            Mathf.Clamp(spherePos.x, aabbCenter.x - aabbExtents.x, aabbCenter.x + aabbExtents.x),
            Mathf.Clamp(spherePos.y, aabbCenter.y - aabbExtents.y, aabbCenter.y + aabbExtents.y),
            Mathf.Clamp(spherePos.z, aabbCenter.z - aabbExtents.z, aabbCenter.z + aabbExtents.z)
        );

        // 最も近い点と球体の中心の距離の二乗を計算
        float distanceSq = (closestPoint - spherePos).sqrMagnitude;

        
        return distanceSq <= (sphereRadius * sphereRadius);
    }
}