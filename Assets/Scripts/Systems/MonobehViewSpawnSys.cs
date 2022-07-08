using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsTestProject.Systems
{
    internal class MonobehViewSpawnSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<SpawnMbViewComp>> _spawnMbViewCompFilter = default;
        private EcsPoolInject<PositionInfoComp> _positionInfoCompPool = default;
        private EcsPoolInject<MonobehViewComp> monobehViewCompPool = default;
        
        public void Run(EcsSystems systems)
        {
            SpawnAndBindViews();
        }

        private void SpawnAndBindViews()
        {
            foreach (var spawningEnt in _spawnMbViewCompFilter.Value)
            {
                ref SpawnMbViewComp spawnComp = ref _spawnMbViewCompFilter.Pools.Inc1.Get(spawningEnt);
                GameObject prefab = spawnComp.Prefab;
                Vector3 spawnPos = spawnComp.SpawnPosition;
                Quaternion spawnRot = spawnComp.SpawnRotation;

                GameObject spawnedGO = GameObject.Instantiate(prefab, spawnPos, spawnRot);
                spawnedGO.gameObject.SetActive(true);
                
                ref MonobehViewComp monobehViewComp = ref monobehViewCompPool.Value.Add(spawningEnt);
                monobehViewComp.ViewTf = spawnedGO.transform;

                if (_positionInfoCompPool.Value.Has(spawningEnt))
                {
                    ref PositionInfoComp positionInfoComp = ref _positionInfoCompPool.Value.Get(spawningEnt);
                    positionInfoComp.Position = Utils.GetNumericsVec3FromUnityVec3(spawnPos);
                    positionInfoComp.Rotation = Utils.GetNumericsQuatFromUnityQuat(spawnRot);
                }
                //TODO Add if animation comp persist

                _spawnMbViewCompFilter.Pools.Inc1.Del(spawningEnt);
            }
        }
    }
}