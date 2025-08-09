using UnityEngine;

public static class CollisionManager
{
    /// <summary>
    /// ���̂�AABB�i�����́j�̏Փ˂𔻒肵�܂��B
    /// </summary>
    /// <param name="spherePos">���̂̒��S���W</param>
    /// <param name="sphereRadius">���̂̔��a</param>
    /// <param name="aabbCenter">AABB�̒��S���W</param>
    /// <param name="aabbExtents">AABB�̔����̃T�C�Y</param>
    /// <returns>�Փ˂��Ă����true</returns>
    public static bool CheckSphereAABB(Vector3 spherePos, float sphereRadius, Vector3 aabbCenter, Vector3 aabbExtents)
    {
        Vector3 closestPoint = new Vector3(
            Mathf.Clamp(spherePos.x, aabbCenter.x - aabbExtents.x, aabbCenter.x + aabbExtents.x),
            Mathf.Clamp(spherePos.y, aabbCenter.y - aabbExtents.y, aabbCenter.y + aabbExtents.y),
            Mathf.Clamp(spherePos.z, aabbCenter.z - aabbExtents.z, aabbCenter.z + aabbExtents.z)
        );

        // �ł��߂��_�Ƌ��̂̒��S�̋����̓����v�Z
        float distanceSq = (closestPoint - spherePos).sqrMagnitude;

        
        return distanceSq <= (sphereRadius * sphereRadius);
    }
}