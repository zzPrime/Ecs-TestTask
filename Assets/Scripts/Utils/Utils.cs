using UnityEngine;

public static class Utils 
{
    public static Vector3 GetUnityVec3FromNumericsVec3(System.Numerics.Vector3 numVec3)
    {
        var unityVec3 = new Vector3(numVec3.X, numVec3.Y, numVec3.Z);
        return unityVec3;
    }
    
    public static System.Numerics.Vector3 GetNumericsVec3FromUnityVec3(Vector3 uniVec3)
    {
        var unityVec3 = new System.Numerics.Vector3(uniVec3.x, uniVec3.y, uniVec3.z);
        return unityVec3;
    }
}
