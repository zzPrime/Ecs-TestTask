using EcsTestProject.Components;
using Leopotam.EcsLite;
using Leopotam.EcsLite.Di;
using UnityEngine;

namespace EcsTestProject.Systems
{
    internal class BindSpawnedMonobehsSys : IEcsRunSystem
    {
        private EcsFilterInject<Inc<MonobehViewComp>, Exc<PositionInfoComp>> _needToBindMonobehFilter;

        private EcsPoolInject<PositionInfoComp> _positionInfoCompPool;
        private EcsPoolInject<AnimationMonobehComp> _animationMonobehCompPool;
        private EcsPoolInject<AnimationStateComp> _animationStateCompPool;

        public void Run(EcsSystems systems)
        {
            BindViewsWithData();
        }

        private void BindViewsWithData()
        {
            foreach (var spawnedEnt in _needToBindMonobehFilter.Value)
            {
                MonobehViewComp monobehViewComp = _needToBindMonobehFilter.Pools.Inc1.Get(spawnedEnt);
                
                ref PositionInfoComp positionInfoComp = ref _positionInfoCompPool.Value.Add(spawnedEnt);

                positionInfoComp.SpawnPosition = Utils.GetNumericsVec3FromUnityVec3(monobehViewComp.ViewTf.position);
                positionInfoComp.Position = positionInfoComp.SpawnPosition;
                positionInfoComp.RotationVector = Utils.GetNumericsVec3FromUnityVec3(monobehViewComp.ViewTf.forward);

                Animator animator = monobehViewComp.ViewTf.GetComponent<Animator>();

                if (animator != null)
                {
                    ref AnimationMonobehComp animationMonobehComp = ref _animationMonobehCompPool.Value.Add(spawnedEnt);
                    animationMonobehComp.Animator = animator;

                    ref AnimationStateComp animationStateComp = ref _animationStateCompPool.Value.Add(spawnedEnt);
                    animationStateComp.AnimationState = AnimationState.Idle;
                }
            }
        }
    }
}