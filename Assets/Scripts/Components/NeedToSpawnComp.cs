using UnityEngine;

namespace EcsTestProject.Components
{
    internal struct SpawnMbViewComp
    {
        public GameObject Prefab;
        public Vector3 SpawnPosition;
        public Quaternion SpawnRotation;
    }
}