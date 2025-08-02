using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static bool CheckSphereSphere(Vector3 pos1, float radius1,Vector3 pos2,float radius2)
    {
        float distance =Vector3.Distance(pos1,pos2);
        return distance <= (radius1 + radius2);
    }


    public static bool CheckSphereAABB (Vector3 spherePos,float sphereRadius,Vector3 aabbCenter,Vector3 aabbExtents)
    {

    }

}
